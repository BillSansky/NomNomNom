using System;
using Nom_Nom_Nom.Placeable;
using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Controller
{
    public class DragAndDropController : AbstractObjectPlacementController
    {
        [SerializeField] private DragAndDropControllerData data;
        [SerializeField] private Camera cam;
        private PlaceableObject placeableObjectToMove;

        [SerializeField] private UnityEvent onDragStarted;
        [SerializeField] private UnityEvent onDragEnded;
        [SerializeField] private UnityEvent onDragCancelled;

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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //drop the object
                controllerData.NotifyNoObjectToHandle();
                onDragEnded.Invoke();
                placeableObjectToMove.NotifyDragDone();
                placeableObjectToMove = null;
                enabled = false;
            }
        }

        private void FixedUpdate()
        {
            if (!placeableObjectToMove)
                return;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, data.GroundLayerMask))
                return;

            placeableObjectToMove.transform.position = hit.point + data.GroundOffset * hit.normal;
        }
    }
}