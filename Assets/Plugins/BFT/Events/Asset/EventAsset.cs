using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace BFT
{
    [CreateAssetMenu(menuName = "BFT/Data/Event/Event Invoker", fileName = "Event Invoker")]
    public class EventAsset : ScriptableObject, IEvent
    {
        [BoxGroup("Status")] [ShowInInspector] [NonSerialized]
        private readonly List<IEventListener> listeners = new();

        private readonly List<IEventListener> listenersToRemoveDuringRaising = new(2);
        
        [FoldoutGroup("Events")] public UnityEvent OnBeforeEventRaised;

        [FoldoutGroup("Events")] public UnityEvent OnEventRaised;

        [NonSerialized] protected bool raising = false;

        public int NumberOfCallsRecorded;
        
        public void Awake()
        {
            ResetNumberOfCallsRecorded();
            listeners.Clear();
            listenersToRemoveDuringRaising.Clear();
        }

        public void ResetNumberOfCallsRecorded()
        {
            NumberOfCallsRecorded = 0;
        }

        [BoxGroup("Utils")]
        [Button(ButtonSizes.Medium)]
        public virtual void RaiseEvent()
        {

            OnBeforeEventRaised.Invoke();

            raising = true;

            NumberOfCallsRecorded++;
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= ResetNumberOfCallsRecordedEditor;
            EditorApplication.playModeStateChanged += ResetNumberOfCallsRecordedEditor;
#endif
            
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
               
                    var objListener = listeners[i] as Object;
                    if (!objListener)
                    {
                        if (listeners[i] == null)
                        {
                            listeners.RemoveAt(i);
                        }
                        continue;
                    }
                    listeners[i].OnEventRaised(this);
              
            }

            raising = false;

            if (listenersToRemoveDuringRaising.Count > 0)
            {
                for (int i = 0; i < listenersToRemoveDuringRaising.Count; i++)
                {
                    listeners.Remove(listenersToRemoveDuringRaising[i]);
                }

                listenersToRemoveDuringRaising.Clear();
            }

            OnEventRaised.Invoke();
        }
        
#if UNITY_EDITOR
        private void ResetNumberOfCallsRecordedEditor(PlayModeStateChange obj)
        {
            ResetNumberOfCallsRecorded();
        }
#endif
        
        public void RegisterListener(IEventListener listener)
        {
#if UNITY_EDITOR

            if (Application.isPlaying)
            {
                EditorApplication.playModeStateChanged -= CheckDataOutOfPlay;
                EditorApplication.playModeStateChanged += CheckDataOutOfPlay;
            }

#endif

            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
                if (raising)
                    listener.OnEventRaised(this);
            }
        }

#if UNITY_EDITOR
        private void CheckDataOutOfPlay(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.ExitingPlayMode:
                case PlayModeStateChange.EnteredEditMode:
                    listeners.Clear();
                    listenersToRemoveDuringRaising.Clear();
                    EditorUtility.SetDirty(this);
                    break;
                case PlayModeStateChange.ExitingEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
            }
        }

#endif


        public void UnRegisterListener(IEventListener listener)
        {
            if (raising)
                listenersToRemoveDuringRaising.Add(listener);
            else
                listeners.Remove(listener);
        }
    }
}