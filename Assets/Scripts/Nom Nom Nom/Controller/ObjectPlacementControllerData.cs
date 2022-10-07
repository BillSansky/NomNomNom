using Nom_Nom_Nom.Placeable;
using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Controller
{
    [CreateAssetMenu(fileName = "Placement Controller Data",
        menuName = "Nom Nom Nom/Controller/Placement Controller Data")]
    public class ObjectPlacementControllerData : ScriptableObject
    {
        [SerializeField] private UnityEvent onNewObjectHandled;
        [SerializeField] private UnityEvent onNoObjectHandled;

        public UnityEvent OnNewObjectHandled => onNewObjectHandled;
        public UnityEvent OnNoObjectHandled => onNoObjectHandled;

        private PlaceableObjectPool placeableObjectPool;

        private PlaceableObject currentHandledObject;

        public PlaceableObject CurrentHandledObject => currentHandledObject;

        public PlaceableObjectPool ObjectPool => placeableObjectPool;


        public void AssignObjectPool(PlaceableObjectPool pool)
        {
            placeableObjectPool = pool;
        }

        public void NotifyNewObjectToHandle(int objID)
        {
            if (!ObjectPool)
            {
                Debug.LogError("The placeable pool was not assigned, the placement cannot be done", this);
                return;
            }

            currentHandledObject = ObjectPool.CreateNewPlaceable(objID);
            currentHandledObject.OnPlaceableDestroyed.AddListener(NotifyCurrentPlaceableDestroyed);
            OnNewObjectHandled.Invoke();
        }

        private void NotifyCurrentPlaceableDestroyed(PlaceableObject placeableObject)
        {
            if (placeableObject == currentHandledObject)
                CancelCurrentObject();
            placeableObject.OnPlaceableDestroyed.RemoveListener(NotifyCurrentPlaceableDestroyed);
        }

        public void CancelCurrentObject()
        {
            if (!ObjectPool)
            {
                Debug.LogError("The placeable pool was not assigned, the placement cancelling cannot be done", this);
                return;
            }

            currentHandledObject.OnPlaceableDestroyed.RemoveListener(NotifyCurrentPlaceableDestroyed);
            ObjectPool.PoolExistingPlaceable(currentHandledObject);
            currentHandledObject = null;
            OnNoObjectHandled.Invoke();
        }

        public void NotifyNoObjectToHandle()
        {
            currentHandledObject.OnPlaceableDestroyed.RemoveListener(NotifyCurrentPlaceableDestroyed);
            currentHandledObject = null;
            OnNoObjectHandled.Invoke();
        }
    }
}