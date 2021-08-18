using UnityEngine;

namespace VIRA.PlayerStats
{

    public static class SaveUtil
    {
        public static void SetObjectValue<T>(string key, T value) where T : class
        {
            string objectValue = (value == null) ? string.Empty : JsonUtility.ToJson(value);
            PlayerPrefs.SetString(key, objectValue);
        }


        public static T GetObjectValue<T>(string key) where T : class
        {
            string savedObjectValue = PlayerPrefs.GetString(key, string.Empty);

            return string.IsNullOrEmpty(savedObjectValue) ? null : JsonUtility.FromJson<T>(savedObjectValue);
        }
    }

}
