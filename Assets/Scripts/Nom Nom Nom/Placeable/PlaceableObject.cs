using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Placeable
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlaceableObject : MonoBehaviour
    {
        [ShowInInspector, BoxGroup("Status"), ReadOnly]
        private int poolId = -1;

        public int PoolId
        {
            get => poolId;
            set => poolId = value;
        }

        public Rigidbody Rb => rb;

        private Rigidbody rb;

        public UnityEvent OnDragStarted;
        public UnityEvent OnDragEnded;

        public PlaceableObjectEvent OnPlaceableDestroyed;
        public PlaceableObjectEvent OnPlaceableCreated;

        private PlaceableObjectPool poolThatSpawnedMe;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void NotifyBeingDragged()
        {
            Rb.isKinematic = true;
            OnDragStarted.Invoke();
        }

        public void NotifyPlacementDone()
        {
            Rb.isKinematic = false;
            OnDragEnded.Invoke();
        }

        public void NotifyPooled()
        {
            Rb.Sleep();
            Rb.isKinematic = true;
            gameObject.SetActive(false);
        }

        public void Destroy()
        {
            poolThatSpawnedMe.PoolExistingPlaceable(this);
            OnPlaceableDestroyed.Invoke(this);
        }

        public void NotifySpawned(PlaceableObjectPool poolThatSpawned)
        {
            poolThatSpawnedMe = poolThatSpawned;
            gameObject.SetActive(true);
            OnPlaceableCreated.Invoke(this);
            Rb.Sleep();
            Rb.isKinematic = true;
        }
    }
}