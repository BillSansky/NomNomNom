using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Utility
{
    public class Counter : MonoBehaviour
    {
        [SerializeField] private int countTo;

        private int currentCount;

        [SerializeField] private UnityEvent onCountDone;

        public void OnEnable()
        {
            currentCount = 0;
        }

        public void Increment()
        {
            currentCount++;
            if (currentCount > countTo)
            {
                onCountDone.Invoke();
                currentCount = 0;
            }
        }
    }
}