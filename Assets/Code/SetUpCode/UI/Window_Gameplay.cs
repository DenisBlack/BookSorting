using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VIRA.WindowsManager;

public class Window_Gameplay : WindowBase
{
    [Header("Debug")]
    [SerializeField] private Button debugWinLevel = default;
    [SerializeField] private Button debugLoseLevel = default;

    [Header("Level")]
    [SerializeField] private TextMeshProUGUI _levelText;
    public override void Hide()
    {
        debugWinLevel.onClick.RemoveListener(GoToResultWin);
        debugLoseLevel.onClick.RemoveListener(GoToResultFail);
        base.Hide();
    }

    public override void Show()
    {
        base.Show();
        debugWinLevel.onClick.AddListener(GoToResultWin);
        debugLoseLevel.onClick.AddListener(GoToResultFail);
        _levelText.text = "LEVEL " + (VIRA.PlayerStats.PlayerStatistics.Level + 1).ToString();
    }

    private void GoToResultWin()
    {
        GameManager.Instance.GoToResult();
    }

    private void GoToResultFail()
    {
        GameManager.Instance.GoToResult(false);
    }
}
