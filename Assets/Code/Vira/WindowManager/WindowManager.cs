using UnityEngine;


namespace VIRA.WindowsManager
{
    public class WindowManager : MonoBehaviourSingleton<WindowManager>
    {
        [SerializeField] WindowBase[] _windows;
        public void Show(Windows window, bool additive = true)
        {
            foreach (WindowBase w in _windows)
            {
                if ((w.window == window))
                {
                    if (w.state == WindowStates.disabled)
                    {
                        w.state = WindowStates.enabled;
                        w.Show();
                    }

                }
                else if (!additive && w.state == WindowStates.enabled)
                {
                    w.state = WindowStates.disabled;
                    w.Hide();
                }
            }
        }

        public void Hide(Windows window)
        {
            foreach (WindowBase w in _windows)
            {
                if (w.window == window && w.state == WindowStates.enabled)
                {
                    w.state = WindowStates.disabled;
                    w.Hide();
                }
            }
        }

        public void SetUp()
        {
            foreach (WindowBase w in _windows)
            {
                if (w.state == WindowStates.enabled)
                {
                    w.Show();
                }
                else if (w.state == WindowStates.disabled)
                {
                    w.Hide();
                }
            }
        }

        public void SetUp(Windows window, bool value)
        {
            foreach (WindowBase w in _windows)
            {
                if (w.window == window)
                {
                    w.SetUp(value);
                }
            }
        }
    }
}
