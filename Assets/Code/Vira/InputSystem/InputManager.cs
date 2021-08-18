using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace VIRA.InputSystem
{
    public class InputManager : MonoBehaviourSingleton<InputManager>
    {
        [Serializable]
        public class Entry
        {
            public event UnityAction<PointerEventData> callback = default;

            public void OnTrigger(PointerEventData eventData)
            {
                callback?.Invoke(eventData);
            }
        }

        [SerializeField] private PointerEventTrigger eventTrigger = default;
        private Dictionary<PointerEventTriggerType, Entry> inputTriggers = new Dictionary<PointerEventTriggerType, Entry>();

        public Entry this[PointerEventTriggerType eventId]
        {
            get { return inputTriggers[eventId]; }
        }

        protected override void Awake()
        {
            base.Awake();
            eventTrigger.triggers.Clear();
            PointerEventTrigger.Entry triggerEntry;
            Entry inputEntry;
            foreach (int i in Enum.GetValues(typeof(PointerEventTriggerType)))
            {
                inputEntry = new Entry();
                triggerEntry = new PointerEventTrigger.Entry();
                triggerEntry.eventID = (PointerEventTriggerType)i;
                triggerEntry.callback.AddListener(inputEntry.OnTrigger);
                eventTrigger.triggers.Add(triggerEntry);
                inputTriggers.Add((PointerEventTriggerType)i, inputEntry);
            }
        }
    }
}