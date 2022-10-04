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

        [SerializeField] private bool useTrigger = false;

        [SerializeField] private bool preventRedetection = true;
        private PlaceableObject lastDetected;

        private void OnTriggerEnter(Collider other)
        {
            if (!useTrigger || !other.attachedRigidbody)
                return;

            DetectionLogic(other.attachedRigidbody);
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (useTrigger)
                return;

            DetectionLogic(collision.body);
        }

        private void DetectionLogic(Component collidee)
        {
            var placeable = collidee.GetComponent<PlaceableObject>();

            if (!placeable)
                return;

            if (preventRedetection && lastDetected == placeable)
                return;

            lastDetected = placeable;

            if (shortListTypesToDetect.Count == 0 || shortListTypesToDetect.Any(_ => _.PoolId == placeable.PoolId))
            {
                onPlaceableObjectDetected.Invoke(placeable);
            }
        }
    }
}