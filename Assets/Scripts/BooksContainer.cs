using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using VIRA.Core.Levels;
using MoreMountains.NiceVibrations;
public class BooksContainer : MonoBehaviour
{
    public BookItem BookItem;
    public ContainerData ContainersData;
    
    public List<BooksContainerSlotData> SlotDatas;
    
    [Header("Settings")] 
    public List<Vector3> ItemsPositions = new List<Vector3>();

    private List<Animator> bookItemAnimators = new List<Animator>();
    private int CurrentAnimationIdx;

    [Header("Editor Settings")]
    public int EditorIndex;

    public bool CanWeSelect = true;
    
    public void SetData(List<ItemData> items)
    {
        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null)
                {
                    var clone = Instantiate(BookItem.transform, transform);
                    clone.transform.localPosition = SlotDatas[i].SlotPosition;

                    var book = clone.GetComponent<BookItem>();
                    book.SetData(items[i]);

                    var freeSlot = SlotDatas.FirstOrDefault(x => x.Item == null);
                    freeSlot.Item = book;
                    freeSlot.ItemData = items[i];
                }
            }
        }
    }

    public Vector3 GetFreePosition()
    {
        return SlotDatas.FirstOrDefault(x => x.Item == null).SlotPosition;
    }
    
    public void SetBook(BookItem bookItem)
    {
        var freeSlot = SlotDatas.FirstOrDefault(x => x.Item == null);
        freeSlot.Item = bookItem;
        freeSlot.ItemData = bookItem.Data;
        UpdateContainerData();
    }
    
    public BookItem GetBook()
    {
        var lastBook = SlotDatas.Last(x=> x.Item != null).Item;
        return lastBook;
    }

    public void ClearLastItemData()
    {
        SlotDatas.Last(x=> x.Item != null).Item = null;
    }
    
    public void UpdateContainerData()
    {
        // ItemsList.Clear();
        //
        // for (int i = 0; i < BookItemsList.Count; i++)
        // {
        //     ItemsList.Add(BookItemsList[i].Data);
        // }
    }
    
    private void OnDrawGizmos()
    {
        // Gizmos.color = new Color(0, 1, 0, 0.2f);
        // Gizmos.DrawCube(transform.position, transform.localScale);
    }

    public bool IsContainerFinished()
    {
        var data = SlotDatas.All(x => x.Item != null);
        if (!data)
            return false;

        var firstElement = SlotDatas.Where(x => x.Item != null).First();
        var sameTypes = SlotDatas.All(x => x.ItemData.ItemType == firstElement.ItemData.ItemType);
            
        return sameTypes;
    }
    
    
    
    public void DoCompletedAnimation()
    {
        var items = SlotDatas.Where(x => x.Item != null).ToList();
        if(items.Count == 0)
            return;
        
        for (int i = 0; i < items.Count; i++)
        {
            var animator = items[i].Item.GetComponent<Animator>();
            if (animator != null)
                bookItemAnimators.Add(animator);
        }
        
        MMVibrationManager.Haptic(HapticTypes.Success);
        
        StartCoroutine(DoPlaySequenceAnimation(CurrentAnimationIdx));
    }

    IEnumerator DoPlaySequenceAnimation(int idx)
    {
        //yield return new WaitForSeconds(0.3f);
        yield return null;
        yield return null;
        
        if (idx != 0)
            yield return new WaitForSeconds(0.06f);
        
        bookItemAnimators[idx].SetTrigger("Success");

        CurrentAnimationIdx++;

        if (CurrentAnimationIdx <= 3)
            StartCoroutine(DoPlaySequenceAnimation(CurrentAnimationIdx));
        else 
            CurrentAnimationIdx = 0;
    }
}

[Serializable]
public class BooksContainerSlotData
{
    public ItemData ItemData;
    public BookItem Item;
    public Vector3 SlotPosition;
}
