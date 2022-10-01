using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Utility
{
    public class AwakeCallback : MonoBehaviour
    {
        public UnityEvent OnAwake;

        private void Awake()
        {
            OnAwake.Invoke();
        }
    }
}