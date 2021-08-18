using System.Linq;
using UnityEditor;
using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject {
    static T _instance = null;
    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                GetInstance();
            }
                
            return _instance;
        }
    }

    private static void GetInstance()
    {
        _instance = Resources.LoadAll<T>("").FirstOrDefault();
        if (_instance == null)
        {
#if UNITY_EDITOR
            var asset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(asset, "Assets/Resources/" + typeof(T).Name +".asset");
            AssetDatabase.SaveAssets();
#endif
        }
    }

#if UNITY_EDITOR
    public static void Select()
    {
        if (Instance == null)
        {
            GetInstance();
        }
        UnityEditor.Selection.activeObject = Instance;
    }
#endif
}