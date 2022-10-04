using System.Collections.Generic;
using System.Linq;
using Nom_Nom_Nom.Controller;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Placeable
{
    public class PlaceableObjectPool : MonoBehaviour
    {
        [SerializeField] private PlaceableObjectCollection collection;

        [SerializeField] private int maxPooledObjects = 20;

        [ShowInInspector, ReadOnly, BoxGroup("Status")]
        private Dictionary<int, List<PlaceableObject>> pooledObjects;

        [ShowInInspector, ReadOnly, BoxGroup("Status")]
        private List<PlaceableObject> activeObjects;

        public UnityEvent OnNewObjectCreated;
        public UnityEvent OnNewObjectPooled;

        public IEnumerable<PlaceableObject> ActiveObjects => activeObjects;

        private void Awake()
        {
            PopulatePool();
        }

        private void PopulatePool()
        {
            pooledObjects = new Dictionary<int, List<PlaceableObject>>(collection.PlaceableObjets.Count());

            int id = 0;
            foreach (var placeableObjet in collection.PlaceableObjets)
            {
                placeableObjet.PlaceableObject.PoolId = id;
                var list = new List<PlaceableObject>(maxPooledObjects);
                for (int i = 0; i < maxPooledObjects; i++)
                {
                    var newObj = Instantiate(placeableObjet.PlaceableObject, transform);
                    PlaceVeryFar(newObj);

                    newObj.PoolId = id;
                    newObj.NotifyPooled();

                    list.Add(newObj);
                }

                pooledObjects.Add(id, list);
                id++;
            }

            activeObjects = new List<PlaceableObject>(maxPooledObjects);
        }

        public PlaceableObject CreateNewPlaceable(int objID)
        {
            if (!pooledObjects.ContainsKey(objID))
            {
                Debug.LogWarning("Warning, trying to create an object that is not part of the pool", this);
                return null;
            }

            var list = pooledObjects[objID];
            if (list.Count == 0)
            {
                var toPool = activeObjects[0];
                toPool.NotifyPooled();
            }

            var toSpawn = list[0];
            PlaceVeryFar(toSpawn);
            list.Remove(toSpawn);
            activeObjects.Add(toSpawn);
            toSpawn.NotifySpawned(this);
            toSpawn.OnPlaceableDestroyed.AddListener(PoolExistingPlaceable);
            OnNewObjectCreated.Invoke();
            return toSpawn;
        }

        public void PoolExistingPlaceable(PlaceableObject obj)
        {
            PlaceVeryFar(obj);
            obj.OnPlaceableDestroyed.RemoveListener(PoolExistingPlaceable);
            activeObjects.Remove(obj);
            pooledObjects[obj.PoolId].Add(obj);
            obj.NotifyPooled();
            OnNewObjectPooled.Invoke();
        }

        private static void PlaceVeryFar(PlaceableObject obj)
        {
            //put very far so it doesnt show for a frame in front
            obj.transform.position = Vector3.one * 9999;
        }
    }
}