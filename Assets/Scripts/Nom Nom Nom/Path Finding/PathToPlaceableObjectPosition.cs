using System;
using System.Collections.Generic;
using System.Linq;
using Nom_Nom_Nom.Controller;
using Nom_Nom_Nom.Placeable;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

namespace Nom_Nom_Nom.Path_Finding
{
    public class PathToPlaceableObjectPosition : AbstractPathProvider
    {
        [SerializeField] private ObjectPlacementControllerData controller;

        private PlaceableObjectPool pool;

        [AssetList] [SerializeField] private List<PlaceableObject> archetypePlaceableToFollow;

        [ShowInInspector, ReadOnly, BoxGroup("Status")]
        private List<PlaceableObject> placeableObjectShortlist = new List<PlaceableObject>();

        [ShowInInspector, ReadOnly, BoxGroup("Status")]
        private PlaceableObject placeableObjectToReach;

        [SerializeField] private bool alwaysPickNearestToTransform;

        [ShowIf("alwaysPickNearestToTransform"), SerializeField]
        private Transform refTransform;

        void OnEnable()
        {
            pool = controller.ObjectPool;
            if (!pool)
                return;


            pool.OnNewObjectCreated.AddListener(RegeneratePlaceablePositions);
            pool.OnNewObjectPooled.AddListener(RegeneratePlaceablePositions);
            RegeneratePlaceablePositions();
        }

        private void OnDisable()
        {
            if (!pool)
                return;

            pool.OnNewObjectCreated.RemoveListener(RegeneratePlaceablePositions);
            pool.OnNewObjectPooled.RemoveListener(RegeneratePlaceablePositions);
        }

        private void RegeneratePlaceablePositions()
        {
            placeableObjectShortlist =
                pool.ActiveObjects.Where(_ => _ != placeableObjectToReach && _.gameObject.activeInHierarchy &&
                                              archetypePlaceableToFollow.Any(arch => arch.PoolId == _.PoolId))
                    .ToList();

            foreach (var placeableObject in placeableObjectShortlist)
            {
                placeableObject.OnPlaceableDestroyed.AddListener(RemovePlaceableFromList);
            }

            if (!placeableObjectToReach)
                GetNextPoint();
        }

        private void RemovePlaceableFromList(PlaceableObject destroyedPlaceable)
        {
            destroyedPlaceable.OnPlaceableDestroyed.RemoveListener(RemovePlaceableFromList);

            if (placeableObjectToReach == destroyedPlaceable)
            {
                GetNextPoint();
            }
            else
            {
                placeableObjectShortlist.Remove(destroyedPlaceable);
            }
        }


        public override Vector3 GetCurrentPointPosition()
        {
            return placeableObjectToReach.transform.position;
        }

        public override void GetNextPoint()
        {
            if (placeableObjectShortlist.Count == 0)
            {
                placeableObjectToReach = null;
                return;
            }

            if (alwaysPickNearestToTransform)
            {
                placeableObjectToReach = placeableObjectShortlist.Where(_ => _.gameObject.activeInHierarchy)
                    .OrderBy(_ => (_.transform.position - refTransform.position).sqrMagnitude).First();
            }
            else
            {
                placeableObjectToReach =
                    placeableObjectShortlist[UnityEngine.Random.Range(0, placeableObjectShortlist.Count)];
            }

            placeableObjectShortlist.Remove(placeableObjectToReach);
        }

        public override bool IsCurrentPointValid()
        {
            return placeableObjectToReach;
        }

        public override bool HasNextPoint()
        {
            return placeableObjectShortlist is { Count: > 0 };
        }
    }
}