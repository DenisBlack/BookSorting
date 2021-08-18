using VIRA.WindowsManager;
using UnityEngine;
using UnityEngine.EventSystems;
using VIRA.InputSystem;

public class Window_Menu : WindowBase
{
    [UnityEngine.SerializeField] private bool _hideByTap = true;
    public override void Hide()
    {
        base.Hide();
        Debug.Log("[WindowSystem] Hide");
        
    }

    public override void Show()
    {
        base.Show();

        if (_hideByTap)
        {
            InputManager.Instance[PointerEventTriggerType.PointerDown].callback += OnPointerDown;
        }
        Debug.Log("[WindowSystem] Show");
    }

    private void OnPointerDown(PointerEventData eventData)
    {
        InputManager.Instance[PointerEventTriggerType.PointerDown].callback -= OnPointerDown;
        Hide();
        GameManager.Instance.MenuHided();
    }

    //private System.Collections.IEnumerator WaitForTap()
    //{

    //    yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
    //    Hide();
    //    GameManager.Instance.MenuHided();

    //}
}
