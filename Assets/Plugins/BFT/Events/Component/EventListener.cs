using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace BFT
{
    public class EventListener : MonoBehaviour, IEventListener
    {
        [BoxGroup("Events"), AssetSelector(Filter = "t:EventAsset")]
        public EventAsset Event;

        [BoxGroup("Events")] public UnityEvent Response;

        [BoxGroup("Options")]
        public bool ListenToPastEventCallsWhileInactive;
        [BoxGroup("Options"),ShowIf("ListenToPastEventCallsWhileInactive")]
        public bool TriggerOncePerPastCall;
        private int previousCallRecorded;

        public void OnEventRaised(IEvent evt)
        {
            if (!enabled)
                return;
            
            previousCallRecorded++;
            if (Response != null)
            {
                Response.Invoke();
            }
        }

        public void OnEnable()
        {
            if (Event)
            {
                if (ListenToPastEventCallsWhileInactive)
                {
                    CheckIfEventWasRaisedWithoutUs();
                }
                Event.RegisterListener(this);
            }
            else
            {
                Debug.LogWarning("Event is null!", this);
            }
        }

        private void CheckIfEventWasRaisedWithoutUs()
        {
            while (previousCallRecorded < Event.NumberOfCallsRecorded)
            {
                if (!TriggerOncePerPastCall)
                {
                    OnEventRaised(Event);
                    return;
                }
                
                OnEventRaised(Event);
                previousCallRecorded++;
            }
        }

        public void OnDisable()
        {
            if (Event != null)
            {
                Event.UnRegisterListener(this);
            }
            else
            {
                UnityEngine.Debug.LogWarning("Event is null!");
            }
        }

        [FoldoutGroup("Tools"), Button("Raise Event Response", ButtonSizes.Medium)]
        public void RaiseEvent()
        {
            OnEventRaised(null);
        }
    }
}