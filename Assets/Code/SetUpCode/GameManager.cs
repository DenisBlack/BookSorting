using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    #region DefaultEvents
    public static event Action<int> LevelStarted = delegate { };            //int = level num
    public static event Action<bool, int, int> LevelFinished = delegate { }; // bool - win/lost + int - place + int - level;
    #endregion

    [Header("TutorialSetUp")]
    [Range(-1, 100)]
    [SerializeField] private int _apearsUntil = -1;
    [SerializeField] private bool _apearsOnlyFirstLvlAfterRestart = false;

    private bool _firstGame = true;


    [Header("ResultSetUp")]
    [SerializeField] private bool _showResultOverGamePlay = false;
    [SerializeField] private bool _restartOnLose = true;

    [Header("Levels")]
    [SerializeField] private VIRA.Core.Levels.LevelsData _levelsData;

    public bool Won { get; private set; }
    public int Place { get; private set; }

    public static event Action<bool> WinEvent = delegate { };

    #region FlowSetUp

    public void MenuHided()
    {
        Play();
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }
    void SetUp()
    {
        LevelController.Instance.SetUpLevel(_levelsData.GetLevel(VIRA.PlayerStats.PlayerStatistics.Level));
        if (_apearsOnlyFirstLvlAfterRestart)
        {
            if (_firstGame)
            {
                _firstGame = false;
                VIRA.WindowsManager.WindowManager.Instance.Show(VIRA.WindowsManager.Windows.Menu);
            }
            else
            {
                MenuHided();
            }
        }
        else if ((_apearsUntil == -1 || VIRA.PlayerStats.PlayerStatistics.Level < _apearsUntil))
        {
            VIRA.WindowsManager.WindowManager.Instance.Show(VIRA.WindowsManager.Windows.Menu);
        }
        else
        {
            MenuHided();
        }
        
    }

    private void Play()
    {
        LevelStarted.Invoke(VIRA.PlayerStats.PlayerStatistics.Level);

        Debug.Log("Lvl started " + VIRA.PlayerStats.PlayerStatistics.Level);
    }

    private void NextLevel()
    {
        VIRA.PlayerStats.PlayerStatistics.UpLevel();
        Debug.Log("Lvl up to " + VIRA.PlayerStats.PlayerStatistics.Level);

    }

    

    public void GoToGamePlay()
    {
        VIRA.WindowsManager.WindowManager.Instance.Show(VIRA.WindowsManager.Windows.Gameplay, false);
        SetUp();
        
        

        
        
    }

    public void GoToResult(bool win = true, int place = 1)
    {
        LevelFinished.Invoke(win, place, VIRA.PlayerStats.PlayerStatistics.Level);
        Won = win;
        WinEvent.Invoke(win);
        Place = place;
        VIRA.WindowsManager.WindowManager.Instance.Show(VIRA.WindowsManager.Windows.Result, _showResultOverGamePlay);
        if (_restartOnLose)
        {
            if (win)
            {
                NextLevel();
            }
        }
        else
        {
            NextLevel();
        }
    }
}
