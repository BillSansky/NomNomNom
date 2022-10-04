using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Utility
{
    public class RegularCall : MonoBehaviour
    {
        [SerializeField] private float callFrequency = 3;

        [SerializeField] private UnityEvent onCall;

        private void OnEnable()
        {
            StartCoroutine(CallRegularly());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator CallRegularly()
        {
            while (true)
            {
                yield return new WaitForSeconds(callFrequency);
                onCall.Invoke();
            }
        }
    }
}