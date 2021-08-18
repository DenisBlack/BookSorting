using System;
using System.Collections;
using System.Collections.Generic;
using JBStateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VIRA.Analytics;
using VIRA.Core.Levels;

public class GameRoot : MonoBehaviourSingleton<GameRoot>
{
    //---------------------------- INSPECTOR ------------------------
    [SerializeField] private LevelBuilderState _levelBuilderState;
    [SerializeField] private PlayerInputState _playerInputState;
    [SerializeField] private EndGameState _endGameState;
        
    private StateMachine<States, Triggers> stateMachine;
    
    public Image VibrationButtonImage;
    
    public event Action<int> LevelStarted = delegate { };            //int = level num
    public event Action<bool, int, int> LevelFinished = delegate { };

    public LevelsSetting CurrentLevelData;
    
    public int CurrentLevel
    {
        get
        {
            if (debugSettings.Enabled)
            {
                if (currentDebugLevel.HasValue == false)
                {
                    currentDebugLevel = debugSettings.Level;
                }
                return currentDebugLevel.Value;
            }
            var level = PlayerPrefs.GetInt("Level");
            return level;
        }
        set
        {
            if (debugSettings.Enabled)
            {
                if (debugSettings.Loop)
                    return;
                currentDebugLevel++;
                return;
            }
            var val = value;
            PlayerPrefs.SetInt("Level", value);
        }
    }
    
    private static int? currentDebugLevel;
    private DebugSettings debugSettings => GameSettings.Instance.DebugSettings;
    
    protected void Start()
    {
        CurrentLevelData = GameSettings.Instance.LevelSettings.Current.GetLevel(CurrentLevel);
        
        var events = FindObjectOfType<LvlEvents>();
        events.Init();
        
        stateMachine = new StateMachine<States, Triggers>(States.Default);
        
        stateMachine.Configure(States.Default, null)
            .Permit(Triggers.GameInitialized, States.LevelInitialization);
        
        
        stateMachine.Configure(States.LevelInitialization, _levelBuilderState)
            .OnGetStateData(transition =>
            {
                return new LevelBuilderState.EnterData()
                {
                    Level = CurrentLevel,
                };
            })
            .OnExitData(transition =>
            {
                if( transition.ExitData is LevelBuilderState.ExitData builderExitData )
                {
                    
                }
            })
            .Permit(Triggers.LevelInitialized, States.PlayerInput)
            .Permit(Triggers.RestartRequest, States.ExitState);
        
        stateMachine.Configure(States.PlayerInput, _playerInputState)
            .OnGetStateData( (transition) => new PlayerInputState.EnterData()
            {
                // Player = Player,
                // Enemies = Enemies,
            })
            .OnExitData((transition) =>
            {
                if (transition.ExitData is PlayerInputState.ExitData exitData)
                {
                    //CurrentTarget = exitData.SelectedEnemy;
                }
            })
            .Permit(Triggers.CanFinishLevel, States.EndGame)
            .Permit(Triggers.RestartRequest, States.ExitState);
        
        stateMachine.Configure(States.EndGame, _endGameState)
            .OnGetStateData(transition =>
            {
                var isWin = true;
                //var isWin = killer == null;
                //LevelFinished.Invoke(isWin, 1, CurrentLevel);
                return new EndGameState.EnterData()
                {
                    IsWin = isWin,
                    Score = 300,
                };
            })
            .Permit(Triggers.RestartRequest, States.ExitState);
        
        stateMachine.Fire(Triggers.GameInitialized);
    }

    public void Fire(Triggers trigger)
    {
        stateMachine.Fire(trigger);
    }
    
    public void SwitchVibration()
    {
        int currentValue = 0;
        var vibrationValue = PlayerPrefs.GetInt("Vibration");
        switch (vibrationValue)
        {
            case 0:
                currentValue = 1;
                break;
            case 1:
                currentValue = 0;
                break;
        }
        PlayerPrefs.SetInt("Vibration", currentValue);

        UpdateVibrationButton();
    }
    
    private void UpdateVibrationButton()
    {
        var vibrationValue = PlayerPrefs.GetInt("Vibration");
        switch (vibrationValue)
        {
            case 0:
                VibrationButtonImage.color = Color.white;
                break;
            case 1:
                VibrationButtonImage.color = Color.gray;
                break;
        }
    }
    
    public void SendLevelStarted()
    {
        LevelStarted.Invoke(CurrentLevel);
    }
    
    public void SendLevelFinished(bool isWin, int place)
    {
        LevelFinished.Invoke(isWin, 1, CurrentLevel);
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene("Main");
    }
    
    public bool CanVibration()
    {
        return PlayerPrefs.GetInt("Vibration") == 0;
    }
    
    public enum States
    {
        Default,
        LevelInitialization,
        PlayerInput,
        EndGame,
        ExitState,
    }

    public enum Triggers
    {
        GameInitialized,
        LevelInitialized,
        CanFinishLevel,
        RestartRequest,
    }
}
