using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace VIRA.InputSystem
{
    [AddComponentMenu("Event/Pointer Event Trigger")]
    public class PointerEventTrigger : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler
    {
        [FormerlySerializedAs("delegates"), SerializeField]
        private List<Entry> m_Delegates;

        protected PointerEventTrigger()
        {
        }

        private void Execute(PointerEventTriggerType id, PointerEventData eventData)
        {
            int num = 0;
            int count = this.triggers.Count;
            while (num < count)
            {
                Entry entry = this.triggers[num];
                if ((entry.eventID == id) && (entry.callback != null))
                {
                    entry.callback.Invoke(eventData);
                }
                num++;
            }
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            this.Execute(PointerEventTriggerType.BeginDrag, eventData);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            this.Execute(PointerEventTriggerType.Drag, eventData);
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            this.Execute(PointerEventTriggerType.Drop, eventData);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            this.Execute(PointerEventTriggerType.EndDrag, eventData);
        }

        public virtual void OnInitializePotentialDrag(PointerEventData eventData)
        {
            this.Execute(PointerEventTriggerType.InitializePotentialDrag, eventData);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            this.Execute(PointerEventTriggerType.PointerClick, eventData);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            this.Execute(PointerEventTriggerType.PointerDown, eventData);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            this.Execute(PointerEventTriggerType.PointerEnter, eventData);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            this.Execute(PointerEventTriggerType.PointerExit, eventData);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            this.Execute(PointerEventTriggerType.PointerUp, eventData);
        }

        public virtual void OnScroll(PointerEventData eventData)
        {
            this.Execute(PointerEventTriggerType.Scroll, eventData);
        }

        [EditorBrowsable((EditorBrowsableState)EditorBrowsableState.Never), Obsolete("Please use triggers instead (UnityUpgradable) -> triggers", true)]
        public List<Entry> delegates
        {
            get => triggers;
            set
            {
                triggers = value;
            }
        }

        public List<Entry> triggers
        {
            get
            {
                if (m_Delegates == null)
                {
                    m_Delegates = new List<Entry>();
                }
                return m_Delegates;
            }
            set
            {
                m_Delegates = value;
            }
        }

        [Serializable]
        public class Entry
        {
            public PointerEventTriggerType eventID = PointerEventTriggerType.PointerClick;
            public PointerEventTrigger.PointerTriggerEvent callback = new PointerEventTrigger.PointerTriggerEvent();
        }

        [Serializable]
        public class PointerTriggerEvent : UnityEvent<PointerEventData>
        {
        }
    }

    public enum PointerEventTriggerType
    {
        PointerEnter = 0,
        PointerExit = 1,
        PointerDown = 2,
        PointerUp = 3,
        PointerClick = 4,
        Drag = 5,
        Drop = 6,
        Scroll = 7,
        InitializePotentialDrag = 12,
        BeginDrag = 13,
        EndDrag = 14,
    }
}