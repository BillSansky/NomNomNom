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
        [SerializeField] private ObjectPlacementController controller;

        private PlaceableObjectPool pool;

        [AssetList] [SerializeField] private List<PlaceableObject> archetypePlaceableToFollow;

        private PlaceableObject[] placeableObjectShortlist;
        private PlaceableObject placeableObjectToReach;

        void OnEnable()
        {
            pool.OnNewObjectCreated.AddListener(RegeneratePlaceablePositions);
            pool.OnNewObjectCreated.AddListener(RegeneratePlaceablePositions);
        }

        private void OnDisable()
        {
            pool.OnNewObjectCreated.RemoveListener(RegeneratePlaceablePositions);
            pool.OnNewObjectCreated.RemoveListener(RegeneratePlaceablePositions);
        }

        private void RegeneratePlaceablePositions()
        {
            placeableObjectShortlist =
                pool.ActiveObjects.Where(_ => archetypePlaceableToFollow.Any(arch => arch.PoolId == _.PoolId))
                    .ToArray();
        }


        public override Vector3 GetNextPoint()
        {
            if (placeableObjectShortlist.Length == 0)
                placeableObjectToReach = null;
            placeableObjectToReach =
                placeableObjectShortlist[UnityEngine.Random.Range(0, placeableObjectShortlist.Length)];

            return placeableObjectToReach.transform.position;
        }

        public override bool HasPointsLeft()
        {
            return placeableObjectShortlist.Length > 0;
        }
    }
}