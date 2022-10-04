using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nom_Nom_Nom.Controller;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Placeable
{
    public class PlaceableObjectSpawner : MonoBehaviour
    {
        [SerializeField] private int amountToSpawn;

        [SerializeField] private List<PlaceableObject> placeableObjectTypesToSpawn;

        [SerializeField] private Vector3 originalPushVector;
        [SerializeField] private float spreadAngle;

        [SerializeField] [MinMaxSlider(0, 1000)]
        private Vector2 forceRange;

        [SerializeField] private ObjectPlacementControllerData controllerData;

        [SerializeField] private Transform spawnTransform;

        private PlaceableObjectPool pool;

        public UnityEvent OnSpawnDone;

        private void OnEnable()
        {
            pool = controllerData.ObjectPool;
        }

        public void Spawn()
        {
            Debug.Assert(placeableObjectTypesToSpawn.Count > 0, "You did not specify placeable Objects to spawn", this);

            StartCoroutine(SpawnMethod());
        }

        private IEnumerator SpawnMethod()
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                var placeableObject = pool.CreateNewPlaceable(
                    placeableObjectTypesToSpawn[UnityEngine.Random.Range(0, placeableObjectTypesToSpawn.Count())]
                        .PoolId);

                placeableObject.NotifyPlacementDone();
                placeableObject.transform.position = spawnTransform.position;

                var forceToAdd = (originalPushVector + AddNoiseOnAngle(0, spreadAngle)) *
                                 UnityEngine.Random.Range(forceRange.x, forceRange.y);
                placeableObject.Rb.AddForce(forceToAdd);
                yield return new WaitForSeconds(0.3f);
            }

            OnSpawnDone.Invoke();
        }

        private Vector3 AddNoiseOnAngle(float min, float max)
        {
            float xNoise = UnityEngine.Random.Range(min, max);
            float yNoise = UnityEngine.Random.Range(min, max);
            float zNoise = UnityEngine.Random.Range(min, max);


            Vector3 noise = new Vector3(
                Mathf.Sin(2 * Mathf.PI * xNoise / 360),
                Mathf.Sin(2 * Mathf.PI * yNoise / 360),
                Mathf.Sin(2 * Mathf.PI * zNoise / 360)
            );
            return noise;
        }
    }
}