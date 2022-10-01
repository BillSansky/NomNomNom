using Nom_Nom_Nom.Placeable;
using UnityEngine;

namespace Nom_Nom_Nom.Controller
{
    public class PlaceableObjectUIController : PlaceableObjectAssignable
    {
        [SerializeField] private ObjectPlacementControllerData controllerData;
        private PlaceableObject placeableObject;


        public void PlaceObject()
        {
            controllerData.NotifyNewObjectToHandle(placeableObject);
        }

        public override void AssignPlaceableObjectDate(PlaceableObjectData obj)
        {
            placeableObject = obj.PlaceableObject;
        }
    }
}
