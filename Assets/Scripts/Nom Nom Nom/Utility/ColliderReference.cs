using UnityEngine;

namespace Nom_Nom_Nom.Utility
{
    [CreateAssetMenu(fileName = "Collider Reference", menuName = "BFT/Ref/Collider")]
    public class ColliderReference : ScriptableObject
    {
        public Collider collider;

        public void SetColliderRef(Collider col)
        {
            collider = col;
        }
    }
}