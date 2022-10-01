using UnityEngine;

namespace Nom_Nom_Nom.Utility
{
    public class TransformUtils : MonoBehaviour
    {
        public void ResetScale()
        {
            transform.localScale = Vector3.one;
        }
    }
}