using System;
using Nom_Nom_Nom.Placeable;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Nom_Nom_Nom.Controller
{
    public class DragAndDropController : AbstractObjectPlacementController
    {
        [SerializeField] private DragAndDropControllerData data;
        [SerializeField] private Camera cam;

        [ShowInInspector, ReadOnly, BoxGroup("Status")]
        private PlaceableObject placeableObjectToMove;

        [SerializeField] private UnityEvent onDragStarted;
        [SerializeField] private UnityEvent onDragEnded;
        [SerializeField] private UnityEvent onDragCancelled;

        [ShowInInspector, ReadOnly, BoxGroup("Status")]
        private Vector2 mousePosition;

        private void OnEnable()
        {
            placeableObjectToMove = controllerData.CurrentHandledObject;
            placeableObjectToMove.NotifyBeingDragged();
            onDragStarted.Invoke();
        }

        private void OnDisable()
        {
            if (placeableObjectToMove)
            {
                //then the script was disabled by something else, clean up stuff
                placeableObjectToMove = null;
                onDragCancelled.Invoke();
            }
        }

        public void UpdateMousePosition(InputAction.CallbackContext context)
        {
            mousePosition = context.ReadValue<Vector2>();
        }

        public void Spawn(InputAction.CallbackContext context)
        {
            if (!enabled)
                return;


            placeableObjectToMove.NotifyPlacementDone();
            placeableObjectToMove = null;
            controllerData.NotifyNoObjectToHandle();
            onDragEnded.Invoke();
        }

        private void FixedUpdate()
        {
            if (!placeableObjectToMove)
                return;
            Ray ray = cam.ScreenPointToRay(mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, data.GroundLayerMask))
                return;

            placeableObjectToMove.transform.position = hit.point + data.GroundOffset * hit.normal;
        }
    }
}