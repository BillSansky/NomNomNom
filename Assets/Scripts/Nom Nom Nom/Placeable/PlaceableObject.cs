using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Placeable
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlaceableObject : MonoBehaviour
    {
        private int poolId;

        public int PoolId
        {
            get => poolId;
            set => poolId = value;
        }

        private Rigidbody rb;

        public UnityEvent OnPlaceableDestroyed;
        public UnityEvent OnPlaceableCreated;
    
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
    
        public void NotifyBeingDragged()
        {
            rb.isKinematic = true;
        }

        public void NotifyDragDone()
        {
            rb.isKinematic = false;
        }

        public void NotifyPooled()
        {
            rb.Sleep();
            rb.isKinematic = true;
            gameObject.SetActive(false);

        }
    
        public void NotifyDestroyed()
        {
            OnPlaceableDestroyed.Invoke();
        }

        public void NotifySpawned()
        {
            gameObject.SetActive(true);
            OnPlaceableCreated.Invoke();
            rb.Sleep();
            rb.isKinematic = true;
        }
    }
}