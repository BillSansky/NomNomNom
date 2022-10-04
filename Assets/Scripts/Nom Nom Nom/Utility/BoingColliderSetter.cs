using BoingKit;
using Nom_Nom_Nom.Controller;
using Nom_Nom_Nom.Placeable;
using UnityEngine;

namespace Nom_Nom_Nom.Utility
{
    public class BoingColliderSetter : MonoBehaviour
    {
        [SerializeField] private ColliderReference refCollider;
        [SerializeField] private BoingBones boingBone;

        private void OnEnable()
        {
            boingBone.UnityColliders = new[] { refCollider.collider };
        }
    }
}