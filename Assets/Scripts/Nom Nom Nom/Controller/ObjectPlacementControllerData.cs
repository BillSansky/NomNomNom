using Nom_Nom_Nom.Placeable;
using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Controller
{
    public class ObjectPlacementControllerData : ScriptableObject
    {
        [SerializeField] private UnityEvent onNewObjectHandled;
        [SerializeField] private UnityEvent onNoObjectHandled;

        public UnityEvent OnNewObjectHandled => onNewObjectHandled;
        public UnityEvent OnNoObjectHandled => onNoObjectHandled;
    
        private PlaceableObjectPool placeableObjectPool;

        private PlaceableObject currentHandledObject;

        public PlaceableObject CurrentHandledObject => currentHandledObject;

   

        public void AssignObjectPool(PlaceableObjectPool pool)
        {
            placeableObjectPool = pool;
        }

        public void NotifyNewObjectToHandle(PlaceableObject obj)
        {
            currentHandledObject = placeableObjectPool.CreateNewPlaceable(obj);
            OnNewObjectHandled.Invoke();
        }

        public void CancelCurrentObject()
        {
            placeableObjectPool.PoolExistingPlaceable(currentHandledObject);
            OnNoObjectHandled.Invoke();
        }

        public void NotifyNoObjectToHandle()
        {
            currentHandledObject = null;
            OnNoObjectHandled.Invoke();
        }
    }
}