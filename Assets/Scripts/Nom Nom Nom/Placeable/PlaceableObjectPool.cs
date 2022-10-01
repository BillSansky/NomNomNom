using System.Collections.Generic;
using System.Linq;
using Nom_Nom_Nom.Controller;
using UnityEngine;

namespace Nom_Nom_Nom.Placeable
{
    public class PlaceableObjectPool : MonoBehaviour
    {
        [SerializeField] private PlaceableObjectCollection collection;
        
        [SerializeField] private int maxPooledObjects = 20;

        private Dictionary<int, List<PlaceableObject>> pooledObjects;

        private List<PlaceableObject> activeObjects;

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
                    newObj.PoolId = id;
                    newObj.NotifyPooled();

                    list.Add(newObj);
                }

                pooledObjects.Add(id, list);
                id++;
            }
        }

        public PlaceableObject CreateNewPlaceable(PlaceableObject obj)
        {
            if (!pooledObjects.ContainsKey(obj.PoolId))
            {
                Debug.LogWarning("Warning, trying to create an object that is not part of the pool", this);
                return null;
            }

            var list = pooledObjects[obj.PoolId];
            if (list.Count == 0)
            {
                var toPool = activeObjects[0];
                toPool.NotifyPooled();
            }

            var toSpawn = list[0];
            list.Remove(toSpawn);
            activeObjects.Add(toSpawn);
            toSpawn.NotifySpawned();
            return toSpawn;
        }

        public void PoolExistingPlaceable(PlaceableObject obj)
        {
            activeObjects.Remove(obj);
            pooledObjects[obj.PoolId].Add(obj);
            obj.NotifyPooled();
        }
    }
}