using UnityEngine;

namespace VIRA.Core.Levels
{
    [CreateAssetMenu(menuName = "Levels/LevelsData")]
    public class LevelsData : ScriptableObject
    {
        public LevelsSetting[] _levels;

        public int Count => _levels.Length;
        public LevelsSetting this[int level] => _levels[ level ];
        public LevelsSetting GetLevel(int level)
        {
            return _levels[level < _levels.Length ? level : Random.Range(5, _levels.Length)];
        }
    }
}

