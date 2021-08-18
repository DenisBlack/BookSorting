using System;
using UnityEngine;
using System.Collections.Generic;
using VIRA.Core.Levels;


public class Source 
{ 

}

[Serializable]
public class SerializableKeyValuePair<K, V>
{
    public K Key;
    public V Value;
}

[Serializable]
public class LevelTable
{
    
    [Serializable]
    public class Level
    {
        public Temp levelType;
        public LevelsData levelData;
    }

    [SerializeField] public List<Level> _level;

    public void SortByLevelType()
    {
        _level.Sort(delegate (Level x, Level y)
        {
            if (x.levelType >= y.levelType)
            {
                return 1;
            }
            else
            {
                return -1;
            }

        });
    }

    public Level GetLevel(Temp levelType)
    {
        int n = (int)levelType;
        if(n >= _level.Count)
        {
            while(n >= _level.Count)
            {
                n--;
            }
            if(_level[n].levelType == levelType)
            {
                return _level[n];
            }
            else
            {
                Debug.LogError("No such categorie");
                return null;
            }
        }
        else 
        {
            int levelC = (int)_level[n].levelType;
            if (levelC > n)
            {
                while (n != 0 && levelType != _level[n].levelType)
                {
                    n--;
                }
            }
            else if (levelC < n)
            {
                int length = _level.Count - 1;
                while (n != length && levelType != _level[n].levelType)
                {
                    n++;
                }

            }
            if (_level[n].levelType == levelType)
            {
                return _level[n];
            }
            else
            {
                Debug.LogError("No such categorie");
                return null;
            }
                
        }
    }
}
