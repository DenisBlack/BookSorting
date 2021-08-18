using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JBStateMachine;
using TMPro;
using UnityEngine;
using VIRA.Core.Levels;

public class LevelBuilderState : MonoBehaviour, IStateController
{
    public TMP_Text LevelLabel;
    public List<Animator> lighting;
    public void OnEntered(EnterDataBase data)
    {
        var enterData = data as EnterData;
        var level = enterData.Level;

        LevelLabel.text = String.Format("Level {0} ", level+1);
        
        //Debug.LogError("Level " + GameRoot.Instance.CurrentLevelData.name + " " + GameRoot.Instance.CurrentLevelData.AmountToWin);

        var currentLevel = GameRoot.Instance.CurrentLevelData; //GameSettings.Instance.LevelSettings.Current.GetLevel(level);

        
        
        foreach (var item in lighting)
        {
            item.Play("Idle",0, UnityEngine.Random.Range(0, 4.6f));
        }
        
        foreach (var item in currentLevel.BookContainers)
        {
            var clone = Instantiate(item.ContainerData.ContainerPrefab);
            clone.transform.position = item.Position;
            clone.transform.rotation = Quaternion.Euler(0,item.Rotation,0);

            var containerController = clone.GetComponent<BooksContainer>();
            if (containerController != null)
            {
                containerController.ContainersData = containerController.ContainersData;
                containerController.SetData(item.ContainerData.ItemsList);
                containerController.EditorIndex = currentLevel.BookContainers.ToList().IndexOf(item);
            }
        }

        GameRoot.Instance.SendLevelStarted();
        GameRoot.Instance.Fire(GameRoot.Triggers.LevelInitialized);
    }
    
    public ExitDataBase OnExited()
    {
        return new ExitData()
        {
           
        };
    }

    public class ExitData : ExitDataBase
    {
  
    }

    public class EnterData : EnterDataBase
    {
        public int Level;
    }
}
