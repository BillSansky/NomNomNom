using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Utility
{
    public class BoundingBoxLimit : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.activeInHierarchy)
            {
                gameObject.transform.position = Vector3.zero;
            }
        }
    }

    public class AwakeCallback : MonoBehaviour
    {
        public UnityEvent OnAwake;

        [SerializeField] private bool waitForOneFrame;

        [Button]
        private void Awake()
        {
            if (waitForOneFrame)
            {
                StartCoroutine(CallInOneFrame());
            }
            else
                OnAwake.Invoke();
        }

        private IEnumerator CallInOneFrame()
        {
            yield return new WaitForEndOfFrame();
            OnAwake.Invoke();
        }
    }
}