using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using VIRA.Core.Levels;

public class GameSettings : SingletonScriptableObject<GameSettings>
{
    [Header("Дебаг")] 
    public DebugSettings DebugSettings;
 
    [Header("Завершение игры")]
    public EndGameSettings EndGameSettings;
    
    [Header("Основные")] 
    public LevelSettings LevelSettings;

    [Header("Префабы")] 
    public Transform BooksContainerPrefab;
    public Transform BookPrefab;
    
    #if UNITY_EDITOR
    [MenuItem("Game/Game Settings")]
    public static void OpenGameSettings()
    {
        GameSettings.Select();
    }
    #endif
}


[System.Serializable]
public class LevelSettings
{
    public LevelsData Current;
}

[System.Serializable]
public class EndGameSettings
{
    public float StopTimeDelay = 0.3F;
    public float ResultScreenDelay = 1.5F;
    public Vector3 CameraOffset;
}

[System.Serializable]
public class DebugSettings
{
    public bool Enabled = false;
    public int Level;
    public bool Loop = true;
}