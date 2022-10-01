using UnityEngine;

namespace Nom_Nom_Nom.Controller
{
    public class ObjectPlacementController : AbstractObjectPlacementController
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                controllerData.CancelCurrentObject();
            }
        }
    }
}