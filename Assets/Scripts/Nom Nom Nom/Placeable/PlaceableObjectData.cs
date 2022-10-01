using Sirenix.OdinInspector;
using UnityEngine;

namespace Nom_Nom_Nom.Placeable
{
    [CreateAssetMenu(fileName = "Placeable Object Data", menuName = "Nom Nom Nom/Placeable Object Data")]
    public class PlaceableObjectData : ScriptableObject
    {
        [SerializeField] private string objectName;
        public string ObjectName => objectName;

        [SerializeField] [AssetsOnly] [AssetList(AutoPopulate = true)]
        private PlaceableObject placeableObject;
        public PlaceableObject PlaceableObject => placeableObject;

    
        [SerializeField] private Texture2D uiRepresentation;

        public Texture2D UIRepresentation => uiRepresentation;

    }
}