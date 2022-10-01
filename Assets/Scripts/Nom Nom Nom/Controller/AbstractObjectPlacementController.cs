using UnityEngine;

namespace Nom_Nom_Nom.Controller
{
    public abstract class AbstractObjectPlacementController : MonoBehaviour
    {
        [SerializeField] protected ObjectPlacementControllerData controllerData;

        protected virtual void Awake()
        {
            //activate only when required
            enabled = false;
            controllerData.OnNewObjectHandled.AddListener(() => enabled = true);
            controllerData.OnNoObjectHandled.AddListener(() => enabled = false);
        }
    }
}