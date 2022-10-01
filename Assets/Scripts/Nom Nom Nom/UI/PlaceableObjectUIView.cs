using Nom_Nom_Nom.Placeable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nom_Nom_Nom.UI
{
    public class PlaceableObjectUIView : PlaceableObjectAssignable
    {
        [SerializeField] private TMP_Text objectName;
        [SerializeField] private RawImage objectImage;
    
        public override void AssignPlaceableObjectDate(PlaceableObjectData obj)
        {
            objectName.text = obj.ObjectName;
            objectImage.texture = obj.UIRepresentation;
        }
    }
}