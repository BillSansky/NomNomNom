using UnityEngine;
using UnityEngine.InputSystem;

namespace Nom_Nom_Nom.Controller
{
    public class ObjectPlacementController : AbstractObjectPlacementController
    {
        public void CancelPlacement(InputAction.CallbackContext context)
        {
            if (enabled)
                controllerData.CancelCurrentObject();
        }
    }
}