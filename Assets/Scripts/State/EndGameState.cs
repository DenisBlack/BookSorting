using System;
using System.Collections;
using System.Collections.Generic;
using JBStateMachine;
using TMPro;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Random = System.Random;

public class EndGameState : MonoBehaviour, IStateController
{
    public GameObject IngamePanel;
    public GameObject WinPane;
    public TMP_Text CompletedLabel;
    public TMP_Text ScoreLabel;

    public float DelayToFinish = 2f;

    [Header("Particles")] 
    public GameObject LeftSideParticles;
    public GameObject RightSideParticles;
    public GameObject MidParticles;
    
    private List<Animator> bookAnimators = new List<Animator>();
    
    public void OnEntered(EnterDataBase data)
    {
        var books = FindObjectsOfType<BookItem>();
        foreach (var item in books)
        {
            var animator = item.GetComponent<Animator>();
            if(animator != null)
                bookAnimators.Add(animator);
        }
        
        StartCoroutine(DelayFinish());
        
        GameRoot.Instance.SendLevelFinished(true,1);
    }

    IEnumerator DelayFinish()
    {
        yield return new WaitForSeconds(1.5f);
        DoRandomPlayBookAnimations();
        
        LeftSideParticles.SetActive(true);
        RightSideParticles.SetActive(true);
        MidParticles.SetActive(true);

        yield return new WaitForSeconds(DelayToFinish);
        
        IngamePanel.SetActive(false);
        WinPane.SetActive(true);
        
        int totalSeconds = (int)Time.timeSinceLevelLoad;
        int seconds = totalSeconds % 60;
        int minutes = totalSeconds / 60;
        string time = minutes > 0 ? minutes + " m " + seconds + " s " : seconds.ToString() +" s";
        ScoreLabel.text = "Level time: " + "\n" + time + "!";
        CompletedLabel.text = "Level " + (GameRoot.Instance.CurrentLevel + 1).ToString() + "\n" + "COMPLETED!";
        
        
        GameRoot.Instance.CurrentLevel++;
    }
    
    public void DoRandomPlayBookAnimations()
    {
        foreach (var item in bookAnimators)
        {
            item.SetTrigger("Success");
        }
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
        public bool IsWin;
        public int Score;
    }
}
