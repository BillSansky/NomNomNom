using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Placeable
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlaceableObject : MonoBehaviour
    {
        private int poolId = -1;

        public int PoolId
        {
            get => poolId;
            set => poolId = value;
        }

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
            rb.isKinematic = true;
            OnDragStarted.Invoke();
        }

        public void NotifyDragDone()
        {
            rb.isKinematic = false;
            OnDragEnded.Invoke();
        }

        public void NotifyPooled()
        {
            rb.Sleep();
            rb.isKinematic = true;
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
            rb.Sleep();
            rb.isKinematic = true;
        }
    }

  
}