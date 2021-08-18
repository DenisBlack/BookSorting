using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VIRA.WindowsManager
{


    public enum WindowStates
    {
        enabled,
        disabled
    }

    public enum Windows
    {
        Menu,
        Gameplay,
        Result
    }

    public class WindowBase : MonoBehaviour
    {

        [Header("SetUp")]
        [SerializeField] protected Canvas windowCanvas = default;
        public WindowStates state;
        public Windows window;


        protected virtual void Start()
        {
            if (state == WindowStates.enabled)
            {
                Show();
            }
            else if (state == WindowStates.disabled)
            {
                Hide();
            }
        }

        public virtual void Show()
        {
            windowCanvas.enabled = true;
        }

        public virtual void Hide()
        {
            windowCanvas.enabled = false;
        }

        public virtual void SetUp(bool result)
        {
            //TODO: Fill or remove
        }

    }

}
