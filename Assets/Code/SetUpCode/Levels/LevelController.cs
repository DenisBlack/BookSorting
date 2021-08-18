using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IClearable
{
    void Clear();

}

public interface IResetable
{
    void ResetObj();
}

public class LevelController : MonoBehaviourSingleton<LevelController>
{
    public void SetUpLevel(VIRA.Core.Levels.LevelsSetting levelSettings)
    {
        ClearLvl();
        ResetObjects();
        BuildLevel(levelSettings);
        Debug.Log("Lvl Setup finished");
    }

    public void BuildLevel(VIRA.Core.Levels.LevelsSetting levelSettings)
    {
        Debug.Log("Lvl built");
    }

    public void ClearLvl()
    {
        Debug.Log("Lvl cleared");
        var clearable = FindObjectsOfType<MonoBehaviour>().OfType<IClearable>();
        foreach(IClearable clear in clearable)
        {
            clear.Clear();
        }
    }

    public void ResetObjects()
    {
        Debug.Log("Lvl objects reset");
        var clearable = FindObjectsOfType<MonoBehaviour>().OfType<IResetable>();
        foreach (IResetable clear in clearable)
        {
            clear.ResetObj();
        }
    }

}


