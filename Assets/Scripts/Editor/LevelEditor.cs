using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using VIRA.Core.Levels;

public class LevelEditor : EditorWindow
{
    private int currentLevel;
    
    [MenuItem("Game/Level Editor")]
    static void Init()
    {
        var window = (LevelEditor) EditorWindow.GetWindow(typeof(LevelEditor));
        window.maxSize = new Vector2(250f, 200f);
        window.minSize = window.maxSize;
        window.Show();
    }

    void OnGUI()
    {
        var levelSettings = GetLevelsSettings();
        
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("Level: ", GUILayout.Width(50));
            var nextLevel = EditorGUILayout.IntField(currentLevel, GUILayout.Width(50));
            if (nextLevel < 0 || nextLevel >= levelSettings.Count)
            {
                nextLevel = currentLevel;
            }
            if (GUILayout.Button("LOAD", GUILayout.Width(100)))
            {
                GUI.FocusControl(null);
                LoadLevel(nextLevel);
            }
        
            currentLevel = nextLevel;
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("<<", GUILayout.Width(50)))
            {
                GUI.FocusControl(null);
                if (currentLevel > 0)
                {
                    currentLevel--;
                    LoadLevel(currentLevel);
                }
            }
            if (GUILayout.Button(">>", GUILayout.Width(50)))
            {
                GUI.FocusControl(null);
                if (currentLevel < levelSettings.Count - 1)
                {
                    currentLevel++;
                    LoadLevel(currentLevel);
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("SAVE", GUILayout.Width(100)))
        {
            SaveCurrentToLevel(currentLevel);
        }
    }

    private void LoadLevel(int level)
    {
        var spawnedEnemies = FindObjectsOfType<BooksContainer>();
        foreach (var spawnedEnemy in spawnedEnemies)
        {
            DestroyImmediate(spawnedEnemy.gameObject);
        }

        
        var levelData = GetLevelData(level);
        var levelBooksData = levelData.BookContainers.ToList();
        foreach (var bookContainersData in levelBooksData)
        {
            var prefab = GameSettings.Instance.BooksContainerPrefab;
            var container = Instantiate(prefab);
            container.transform.position = bookContainersData.Position;
            container.eulerAngles = new Vector3(0, bookContainersData.Rotation, 0);
       
            var controller = container.GetComponent<BooksContainer>();
            controller.EditorIndex = levelBooksData.IndexOf(bookContainersData);
            controller.ContainersData = bookContainersData.ContainerData;

            for (int i = 0; i < bookContainersData.ContainerData.ItemsList.Count; i++)
            {
                controller.SlotDatas[i].ItemData = bookContainersData.ContainerData.ItemsList[i];
            }
            
            if(!bookContainersData.ContainerData.IsEmptyContainerData)
                SetContainerData(controller, container);
            
            //controller.SetData(bookContainersData.ContainerData.ItemsList);
         
        }
    }

    private void SetContainerData(BooksContainer container, Transform parentTransform)
    {
        var bookList = container.ContainersData.ItemsList;
        int idx = 0;
        foreach (var item in bookList)
        {
            var prefab = GameSettings.Instance.BookPrefab;
            var clone = Instantiate( prefab );
            clone.SetParent(parentTransform);
            clone.transform.localPosition = container.SlotDatas[idx].SlotPosition;
            clone.transform.localRotation = Quaternion.Euler(0,0,0);

            Debug.Log("color: " + item.ColorValue);
            
            var meshRenderer = clone.GetComponent<BookItem>().BookGO.GetComponent<MeshRenderer>();
            meshRenderer.material.SetFloat("_hue", item.ColorValue);
            
            idx++;
        }
    }
    
    private void SaveCurrentToLevel(int level)
    {
        var booksContainers = FindObjectsOfType<BooksContainer>().OrderBy(x=> x.EditorIndex);
        var levelData = GetLevelData(level);
        
        levelData.BookContainers = booksContainers.Select(
            x => new BookContainersData()
        {
            ContainerData = x.ContainersData,
            Position = x.transform.position,
            Rotation = x.transform.eulerAngles.y
            
        }).ToArray();
        
        EditorUtility.SetDirty(levelData);
        AssetDatabase.SaveAssets();
    }
    
    private LevelsSetting GetLevelData(int level)
    {
        return GetLevelsSettings()[ level ];
    }

    private LevelsData GetLevelsSettings()
    {
        return GameSettings.Instance.LevelSettings.Current;
    }
}
