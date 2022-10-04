using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Utility
{
    public class Counter : MonoBehaviour
    {
        [SerializeField] private int countTo;
        [SerializeField] private int initialCount;

        [ShowInInspector,ReadOnly,BoxGroup("Status")]
        private int currentCount;

        [SerializeField] private UnityEvent onCountDone;

        public void OnEnable()
        {
            ResetCount();
        }

        public void ResetCount()
        {
            currentCount = initialCount;
        }

        public void Increment()
        {
            currentCount++;

            if (currentCount == countTo)
            {
                onCountDone.Invoke();
            }
        }

        public void Decrement()
        {
            currentCount--;

            if (currentCount == countTo)
            {
                onCountDone.Invoke();
            }
        }
    }
}