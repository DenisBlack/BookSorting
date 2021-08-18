using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    public List<ContainerData> containersData;
    public List<BooksContainer> BooksContainers;
    
    //
    public BooksContainer SelectedBooksContainer;

    private Coroutine inputRoutine;
    
    void Start()
    {
        for (int i = 0; i < BooksContainers.Count; i++)
        {
            BooksContainers[i].SetData(containersData[i].ItemsList);
        }

        inputRoutine = StartCoroutine(DoInput());
    }

    IEnumerator DoInput()
    {
        while (true)
        {
            while (SelectedBooksContainer == null)
            {
                if (Input.GetMouseButton(0))
                    SelectedBooksContainer = GetCurrentBooksContainer(ContainerState.GetBook);

                yield return null;
            }
            
            Debug.Log("Select");
            
            var selectedBook = SelectedBooksContainer.GetBook();
            selectedBook.Select();

            SelectedBooksContainer.ClearLastItemData();
            SelectedBooksContainer = null;
            
            while (!selectedBook.AnimationPlayed)
                yield return null;
            
            //yield return new WaitForSeconds(0.3f);

            while (SelectedBooksContainer == null)
            {
                if (Input.GetMouseButton(0))
                    SelectedBooksContainer = GetCurrentBooksContainer(ContainerState.SetBook);

                yield return null;
            }

            var position =  SelectedBooksContainer.GetFreePosition();
            SelectedBooksContainer.SetBook(selectedBook);
            selectedBook.Unselect(SelectedBooksContainer, position);
            
            while (!selectedBook.AnimationPlayed)
                yield return null;
            
            //yield return new WaitForSeconds(0.3f);

            SelectedBooksContainer = null;

            yield return null;
            yield return null;
            yield return null;
        }
    }
    private BooksContainer GetCurrentBooksContainer(ContainerState state)
    {
        var worldPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(worldPosition, out var hit, Mathf.Infinity))
        {
            switch (state)
            {
                case ContainerState.GetBook:
                    var getBooksContainer = hit.transform.GetComponent<BooksContainer>();
                    if (getBooksContainer != null && getBooksContainer.SlotDatas.Count(x=> x.Item != null) > 0)
                    {
                        return getBooksContainer;
                    }
                    break;
                case ContainerState.SetBook:
                    var setBooksContainer = hit.transform.GetComponent<BooksContainer>();
                    if (setBooksContainer != null && setBooksContainer.SlotDatas.Count(x=> x.Item != null) < 4)
                    {
                        return setBooksContainer;
                    }
                    break;
            }
        }
        
        return null;
    }
    
    public enum ContainerState
    {
        GetBook,
        SetBook
    }
}
