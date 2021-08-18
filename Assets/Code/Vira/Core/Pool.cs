using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class for storing deactivated game objects and their returning on request
/// </summary>
public class Pool : MonoBehaviourSingleton<Pool>
{
    /// <summary>
    /// contains all prefabs to instantiate
    /// </summary>
    [SerializeField]
    private List<MonoBehaviour> IPullablePrefabs = new List<MonoBehaviour>();

    /// <summary>
    /// list keys (UnitType enums) to lists of instantiated prefabs
    /// </summary>
    private Dictionary<UnitType, List<IPoolable>> pullableLists = new Dictionary<UnitType, List<IPoolable>>();

    /// <summary>
    /// add unit(in all states) to the pull. 
    /// </summary>
    /// <param name="unit"></param>
    public void Add(IPoolable unit)
    {
        unit.Off();

        List<IPoolable> list;

        if (!pullableLists.TryGetValue(unit.Type, out list))
        {
            list = new List<IPoolable>();
            pullableLists.Add(unit.Type, list);
        }
        list.Add(unit);
    }

    /// <summary>
    /// returns unit of a given type
    /// </summary>
    /// <typeparam name="T">class of the desired unit</typeparam>
    /// <param name="type">type of the desired unit</param>
    /// <returns>object of the desired class and type or error message</returns>
    public T Get<T>(UnitType type, Vector3 localscale, Transform parent = null) where T : MonoBehaviour
    {
        List<IPoolable> list;
        if (pullableLists.TryGetValue(type, out list))
        {
            if (list.Count > 0)
            {
                IPoolable obj = list[0];
                list.Remove(obj);
                obj.Reset();

                if (parent && obj.Transform)
                {
                    (obj as T).gameObject.transform.SetParent(parent);
                    (obj as T).gameObject.transform.localScale = localscale;
                }

                return obj as T;
            }
        }

        MonoBehaviour prefab = IPullablePrefabs.Find((obj) =>
        {
            return obj is T && obj is IPoolable && (obj as IPoolable).Type == type;
        });

        if (prefab != null)
        {
            T instanse = Instantiate(prefab as T);
            (instanse as IPoolable).Reset();
            if (parent)
            {
                (instanse as IPoolable).Transform?.SetParent(parent);
            }
            return instanse;
        }
        else
        {
            Debug.LogWarning("prefab not found");
        }

        return null;
    }
}
