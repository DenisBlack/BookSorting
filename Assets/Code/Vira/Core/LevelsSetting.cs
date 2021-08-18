using System;
using UnityEngine;

namespace VIRA.Core.Levels
{
    [CreateAssetMenu(menuName = "Levels/LevelsSetting", fileName = "LevelsSettingData")]
    public class LevelsSetting : ScriptableObject
    {
        public BookContainersData[] BookContainers;
        public int AmountToWin;
    }

    [Serializable]
    public class BookContainersData
    {
        public ContainerData ContainerData;
        public Vector3 Position;
        public float Rotation;
    }
}

