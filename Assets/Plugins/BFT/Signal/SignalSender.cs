using UnityEngine;

namespace Plugins.BFT.Signal
{
    public class SignalSender : MonoBehaviour
    {
        [SerializeField] private SignalType signalType;

        public void SendSignalToObject(GameObject go)
        {
            var receiver = go.GetComponentInChildren<SignalReceiver>();
            if (receiver)
                receiver.ReceiveSignal(signalType);
            
        }
    }
}