using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class BookItem : MonoBehaviour, ISelectable
{
    public ItemData Data;
    public ItemEnum ItemType;

    [Header("Settings")] 
    public Transform BookGO;
    private MeshRenderer _meshRenderer;
    private Vector3 defoultScale;

    private Animator _animator;
    public bool AnimationPlayed;

    public int startContainerIndex;
    public int endContainerIndex;
    
    public Vector2 startPosContainer;
    public Vector2 endPosContainer;

    public bool UsingBook;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetData(ItemData data)
    {
        if (data != null)
            Data = data;
        
        ItemType = data.ItemType;
        
        _meshRenderer = BookGO.GetComponent<MeshRenderer>();
        _meshRenderer.material.SetFloat("_hue", data.ColorValue);

        defoultScale = transform.localScale;
    }
    
    public void Select()
    {
        UsingBook = true;
        
        AnimationPlayed = false;
        
        transform.SetParent(null);

        _animator.SetTrigger("PickUp");

        StartCoroutine(AnimationDelay(0.25f));
    }
    public void Unselect(BooksContainer container, Vector3 freeSlotPosition)
    {
        AnimationPlayed = false;
        
        transform.SetParent(container.transform,true);
        transform.localScale = defoultScale;

        _animator.SetTrigger("Placement");
        
        StartCoroutine(AnimationDelay(0.2f));
    }

    public IEnumerator UnselectCoroutine(BooksContainer container, Vector3 freeSlotPosition)
    {
        container.CanWeSelect = false;
     
        AnimationPlayed = false;
        
        yield return null;

        bool canNext = false;

        if (IsTheSameContainer())
        { 
            transform.SetParent(container.transform,true);
            transform.localScale = defoultScale;
            
            _animator.SetTrigger("Placement");
            yield return new WaitForSeconds(0.2f);
            
            if(GameRoot.Instance.CanVibration())
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
            
            container.CanWeSelect = true;
            
            if(container.IsContainerFinished())
                container.Invoke("DoCompletedAnimation", 0.2f);
            
            ClearSomeData();
            yield break;
        }
 
        transform.DOMove(GetMiddlePosition(), 0.25f).OnComplete(() =>
        {
            canNext = true;
        });

        AnimationPlayed = true;
        
        var animationPos = GetAxisAnimation();
        
        _animator.SetFloat("Vertical", animationPos.Item2);
        _animator.SetFloat("Horizontal", animationPos.Item1);
        _animator.SetTrigger("Transporting");
        
        while (!canNext)
            yield return null;
        
        transform.SetParent(container.transform,true);
        transform.localScale = defoultScale;

        transform.DOLocalMove(freeSlotPosition, 0.2f).OnComplete(() =>
        {
            _animator.SetTrigger("Placement");
            
            if(GameRoot.Instance.CanVibration())
            MMVibrationManager.Haptic(HapticTypes.LightImpact);

            container.CanWeSelect = true;
            
            if(container.IsContainerFinished())
                container.Invoke("DoCompletedAnimation", 0.2f);
            
            ClearSomeData();
        });

        UsingBook = false;
        
        yield return null;
    }

    private void IsReadyContainer(BooksContainer container)
    {
        
    }
    

    private void ClearSomeData()
    {
        startPosContainer = Vector3.zero;
        endPosContainer = Vector3.zero;

        startContainerIndex = -1;
        endContainerIndex = -1;
        
        AnimationPlayed = true;
    }

    private Vector2 GetMiddlePosition()
    {
        var pos = (endPosContainer - startPosContainer) + startPosContainer;
        return pos;
    }

    private bool IsTheSameContainer()
    {
        var result = startContainerIndex == endContainerIndex;
        return result;
    }

    public Tuple<float,float> GetAxisAnimation()
    {
        var x = endPosContainer.x < startPosContainer.x;
        var y = endPosContainer.y < startPosContainer.y;

        float xAxis = 0;
        float yAxis = 0;
        
        if (endPosContainer.x < startPosContainer.x)
            xAxis = -1f;
        else if(endPosContainer.x > startPosContainer.x)
            xAxis = 1f;
        else if(endPosContainer.x == startPosContainer.x)
            xAxis = 0f;

        if (endPosContainer.y < startPosContainer.y)
            yAxis = -1f;
        else if(endPosContainer.y > startPosContainer.y)
            yAxis = 1f;
        else if(endPosContainer.y == startPosContainer.y)
            yAxis = 0f;
        
        return Tuple.Create(xAxis,yAxis);
    }
    
    IEnumerator AnimationDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        AnimationPlayed = true;
    }

    public IEnumerator DoPlaySuccesAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.SetTrigger("Success");
    }
    
    public void Unselect()
    {
        
    }
}
