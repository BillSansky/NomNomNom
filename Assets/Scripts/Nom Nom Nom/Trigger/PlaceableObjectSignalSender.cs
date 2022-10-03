using Nom_Nom_Nom.Placeable;
using Plugins.BFT.Signal;
using UnityEngine;

namespace Nom_Nom_Nom.Trigger
{
    public class PlaceableObjectSignalSender : MonoBehaviour
    {
        public SignalType SignalToSend;

        public void SendSignal(PlaceableObject placeableObject)
        {
            var receiver = placeableObject.GetComponent<SignalReceiver>();

            if (receiver)
                receiver.ReceiveSignal(SignalToSend);
        }
    }
}