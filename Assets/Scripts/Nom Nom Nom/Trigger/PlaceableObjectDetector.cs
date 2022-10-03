using System;
using System.Collections.Generic;
using System.Linq;
using Nom_Nom_Nom.Placeable;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Trigger
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlaceableObjectDetector : MonoBehaviour
    {
        [SerializeField] private PlaceableObjectEvent onPlaceableObjectDetected;

        [SerializeField] [AssetList] private List<PlaceableObject> shortListTypesToDetect;

        private void OnCollisionEnter(Collision collision)
        {
            var placeable = collision.body.GetComponent<PlaceableObject>();

            if (!placeable)
                return;

            if (shortListTypesToDetect.Count == 0 || shortListTypesToDetect.Any(_ => _.PoolId == placeable.PoolId))
            {
                onPlaceableObjectDetected.Invoke(placeable);
            }
        }
    }
}