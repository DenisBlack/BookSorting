using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Data/Item Data")]
public class ItemData : ScriptableObject
{
    public Transform Prefab;
    public Color ItemColor;

    public float ColorValue;
    
    public ItemEnum ItemType;
}

[Serializable] public enum ItemEnum
{
    Blue,
    Red,
    Purple,
    Green,
    Orange,
    Yellow,
}
