using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Nom_Nom_Nom.Trigger
{
    public class AnyCollisionDetector : MonoBehaviour
    {
        [FormerlySerializedAs("OnCollisionDetected")]
        public UnityEvent OnCollisionEntered;

        public UnityEvent OnCollisionExited;


        [SerializeField] private LayerMask collisionMask;

        [SerializeField] private bool useTrigger;

        [SerializeField] private List<Collider> collidersToIgnore;

        private static bool IsLayerInMask(LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (enabled && !useTrigger &&
                IsLayerInMask(collisionMask, collision.body.gameObject.layer) &&
                collidersToIgnore.All(_ => _ != collision.collider))
            {
                OnCollisionEntered.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (enabled && useTrigger && IsLayerInMask(collisionMask, other.gameObject.layer) &&
                collidersToIgnore.All(_ => _ != other))
            {
                OnCollisionEntered.Invoke();
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (enabled && !useTrigger &&
                IsLayerInMask(collisionMask, collision.body.gameObject.layer) &&
                collidersToIgnore.All(_ => _ != collision.collider))
            {
                OnCollisionExited.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (enabled && useTrigger && IsLayerInMask(collisionMask, other.gameObject.layer) &&
                collidersToIgnore.All(_ => _ != other))
            {
                OnCollisionExited.Invoke();
            }
        }
    }
}