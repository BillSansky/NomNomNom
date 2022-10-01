using UnityEngine;

namespace Nom_Nom_Nom.Controller
{
    [CreateAssetMenu(fileName = "Drag And Drop Controller Data", menuName = "Drag And Drop Controller Data")]
    public class DragAndDropControllerData : ScriptableObject
    {
        [SerializeField] private float groundOffset;
        [SerializeField] private LayerMask groundLayerMask;

        public float GroundOffset => groundOffset;
        public LayerMask GroundLayerMask => groundLayerMask;
    } 
}
