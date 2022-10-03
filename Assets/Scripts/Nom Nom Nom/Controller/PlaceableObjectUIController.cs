using System;
using Nom_Nom_Nom.Placeable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nom_Nom_Nom.Controller
{
    public class PlaceableObjectUIController : PlaceableObjectAssignable
    {
        [SerializeField] private ObjectPlacementControllerData controllerData;
        [ReadOnly,SerializeField] private int placeableObjectID = -1;

        public void PlaceObject()
        {
            Debug.Assert(placeableObjectID != -1, "the id wasn't properly initialized");
            controllerData.NotifyNewObjectToHandle(placeableObjectID);
        }

        public override void AssignPlaceableObjectDate(PlaceableObjectData obj)
        {
            placeableObjectID = obj.PlaceableObject.PoolId;
        }
    }
}