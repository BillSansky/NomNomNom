using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Plugins.BFT.Signal
{
    [CreateAssetMenu(fileName = "Signal", menuName = "BFT/Signal")]
    public class SignalType : ScriptableObject
    {
    }


    public class SignalReceiver : MonoBehaviour
    {
        [Serializable]
        public class SignalEvent
        {
            [SerializeField] private SignalType SignalType;
            [SerializeField] private UnityEvent OnSignalReceived;

            public void TryToReceiveSignal(SignalType type)
            {
                if (type == SignalType)
                {
                    OnSignalReceived.Invoke();
                }
            }
        }

        [SerializeField] private List<SignalEvent> SignalEvents;

        public void ReceiveSignal(SignalType type)
        {
            foreach (var signalEvent in SignalEvents)
            {
                signalEvent.TryToReceiveSignal(type);
            }
        }
    }
}