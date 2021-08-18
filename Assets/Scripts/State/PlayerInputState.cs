using System;
using System.Collections;
using System.Collections.Generic;
using JBStateMachine;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using VIRA.Core.Levels;
using MoreMountains.NiceVibrations;
public class PlayerInputState : MonoBehaviour, IStateController
{

    private BooksContainer[] allBookContainers;
    private LevelsSetting CurrentLevel;
    
    public BookItem SelectedBook;
    public BooksContainer SelectedBooksContainer;
    private Coroutine inputRoutine;
    
    [Header("Tutorial")] 
    public int TutorialStepID;
    public List<TutorialData> TutorialDatas;
    public GameObject TutorialWindow;
    public RectTransform TutorialHand;
    
    public Vector3 HandOffset_Book;
    public Vector3 HandOffset_Container;
    
    private ContainerState CurrentState;
    public void OnEntered(EnterDataBase data)
    {
        inputRoutine = StartCoroutine(DoSelect());
        
        allBookContainers = FindObjectsOfType<BooksContainer>();

        CurrentLevel = GameRoot.Instance.CurrentLevelData;
        
        bool IsTutorial = GameRoot.Instance.CurrentLevel == 0;
        if (IsTutorial)
        {
            CurrentState = ContainerState.GetBook;
            StartCoroutine(DoTutorial());
        }
    }

    IEnumerator DoSelect()
    {
        CurrentState = ContainerState.GetBook;
        
        while (SelectedBooksContainer == null)
        {
            if (Input.GetMouseButton(0))
                SelectedBooksContainer = GetCurrentBooksContainer(ContainerState.GetBook);

            yield return null;
        }

        if (GameRoot.Instance.CanVibration())
        {
            MMVibrationManager.SetHapticsActive(true);
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }

        //Debug.Log("Select");
        
        SelectedBook = SelectedBooksContainer.GetBook();
        SelectedBook.Select();
        SelectedBook.startContainerIndex = SelectedBooksContainer.EditorIndex;
        SelectedBook.startPosContainer = SelectedBooksContainer.transform.position;
        
        SelectedBooksContainer.ClearLastItemData();
        
        while (!SelectedBook.AnimationPlayed)
            yield return null;
        
        SelectedBooksContainer = null;
        
        TutorialStepID++;
        
        StartCoroutine(DoUnSelect());
    }

    IEnumerator DoUnSelect()
    {
        CurrentState = ContainerState.SetBook;
        
        while (SelectedBooksContainer == null)
        {
            if (Input.GetMouseButton(0))
                SelectedBooksContainer = GetCurrentBooksContainer(ContainerState.SetBook);

            yield return null;
        }

        //Debug.Log("UnSelect");
        
        SelectedBook.endPosContainer = SelectedBooksContainer.transform.position;
        SelectedBook.endContainerIndex = SelectedBooksContainer.EditorIndex;
        
        var position =  SelectedBooksContainer.GetFreePosition();
        SelectedBooksContainer.SetBook(SelectedBook);

        SelectedBook.StartCoroutine(SelectedBook.UnselectCoroutine(SelectedBooksContainer, position));
        
        while (!SelectedBook.AnimationPlayed)
            yield return null;

        // var isReadyContainer = SelectedBooksContainer.IsContainerFinished();
        // if (isReadyContainer)
        // {
        //     SelectedBooksContainer.Invoke("DoCompletedAnimation", 0.2f);
        // }

        SelectedBooksContainer = null;

        CurrentState = ContainerState.GetBook;
        TutorialStepID++;
        
        CanWeFinish();
    }

    private void CanWeFinish()
    {
        List<bool> completedContainers = new List<bool>();
        for (int i = 0; i < allBookContainers.Length; i++)
        {
            completedContainers.Add(allBookContainers[i].IsContainerFinished());
        }

        if (completedContainers.Count(x => x == true) == CurrentLevel.AmountToWin)
        {
            OffAllContainer();
            GameRoot.Instance.Fire(GameRoot.Triggers.CanFinishLevel);
        }
        else StartCoroutine(DoSelect());
    }
    
    private BooksContainer GetCurrentBooksContainer(ContainerState state)
    {
        bool IsTutorial = GameRoot.Instance.CurrentLevel == 0;
        
        var worldPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(worldPosition, out var hit, Mathf.Infinity))
        {
            switch (state)
            {
                case ContainerState.GetBook:
                    var getBooksContainer = hit.transform.GetComponent<BooksContainer>();
                    if (getBooksContainer != null && getBooksContainer.CanWeSelect && getBooksContainer.SlotDatas.Count(x=> x.Item != null) > 0)
                    {
                        if (IsTutorial && getBooksContainer.EditorIndex != GetTutorialContainer().EditorIndex)
                            return null;
                            
                        return getBooksContainer;
                    }
                    break;
                case ContainerState.SetBook:
                    var setBooksContainer = hit.transform.GetComponent<BooksContainer>();
                    if (setBooksContainer != null && setBooksContainer.SlotDatas.Count(x=> x.Item != null) < 4)
                    {
                        if (IsTutorial && setBooksContainer.EditorIndex != GetTutorialContainer().EditorIndex)
                            return null;
                        
                        return setBooksContainer;
                    }
                    else if (setBooksContainer != null && setBooksContainer.SlotDatas.Count(x => x.Item != null) == 4)
                    {
                        if (GameRoot.Instance.CanVibration())
                            MMVibrationManager.Haptic(HapticTypes.Warning);

                        SelectedBook.GetComponent<Animator>().CrossFade("Nope", 0, 0,0f);
                    }
                    break;
            }
        }
        return null;
    }

    private BooksContainer GetTutorialContainer()
    {
        var containers = FindObjectsOfType<BooksContainer>().OrderBy(x => x.EditorIndex).ToList();
        foreach (var item in containers)
        {
            var collider = item.GetComponent<Collider>();
            if (collider != null)
                collider.enabled = false;
        }

        int targetData = 0;
        try
        {
            targetData = TutorialDatas.FirstOrDefault(x => x.StepID == TutorialStepID).TargetID;
        }
        catch (Exception e)
        {
            TutorialWindow.SetActive(false);
            StopCoroutine(DoTutorial());
            Console.WriteLine(e);
        }

        containers[targetData].GetComponent<Collider>().enabled = true;
        
        return containers[targetData];
    }

    void OffAllContainer()
    {
        var containers = FindObjectsOfType<BooksContainer>().OrderBy(x => x.EditorIndex).ToList();
        foreach (var item in containers)
        {
            var collider = item.GetComponent<Collider>();
            if (collider != null)
                collider.enabled = false;
        }
    }
    
    IEnumerator DoTutorial()
    {
        TutorialWindow.SetActive(true);

        while (GameRoot.Instance.CurrentLevel == 0)
        {
            int currentStep = TutorialStepID;
            
            var currentContainer = GetTutorialContainer();

            BooksContainerSlotData currentBook = null;
            Vector3 pos = Vector3.zero;
            
            if (CurrentState == ContainerState.GetBook)
            {
                try
                {
                    currentBook = currentContainer.SlotDatas.Where(x => x.Item != null).ToList().LastOrDefault();
                    if (currentBook != null && currentBook.Item.UsingBook)
                        currentBook = null;

                    pos = Camera.main.WorldToScreenPoint(currentBook.Item.transform.position);
                    TutorialHand.position = pos + HandOffset_Book;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                pos = Camera.main.WorldToScreenPoint(currentContainer.transform.position);
                TutorialHand.position = pos + HandOffset_Container;
            }

            while (currentStep == TutorialStepID)
                yield return null;
            
            yield return null;
        }
        
        TutorialWindow.SetActive(false);
    }
    
    
    [Serializable]
    public class TutorialData
    {
        public int StepID;
        public int TargetID;
    }
    
    public enum ContainerState
    {
        GetBook,
        SetBook
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
