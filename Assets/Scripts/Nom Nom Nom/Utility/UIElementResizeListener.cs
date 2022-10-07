using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Utility
{
    public class UIElementResizeListener : MonoBehaviour
    {
        public UnityEvent OnRectDimensionChanged;

        private void OnRectTransformDimensionsChange()
        {
            OnRectDimensionChanged.Invoke();
        }
    }
}