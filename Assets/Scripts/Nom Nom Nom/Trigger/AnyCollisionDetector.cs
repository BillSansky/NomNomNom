using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Trigger
{
    public class AnyCollisionDetector : MonoBehaviour
    {
        public UnityEvent OnCollisionDetected;
        
        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionDetected.Invoke();
        }
    }
}