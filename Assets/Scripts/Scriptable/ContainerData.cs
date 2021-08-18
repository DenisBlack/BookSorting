using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Container Data", menuName = "Data/Container Data")]
public class ContainerData : ScriptableObject
{
    public bool IsEmptyContainerData;
    public Transform ContainerPrefab;
    public List<ItemData> ItemsList;
}
