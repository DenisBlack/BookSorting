using System;
using UnityEngine;

namespace VIRA.PlayerStats
{


    public static class PlayerStatistics
    {
        public static event Action<int> KeyUpdated = delegate { };
        public static event Action<int, Vector3> MoneyUpdated = delegate { };


        [Serializable]
        public class Data
        {
            public int level = default;
            public int totalStars = default;
            public int money = default;
            public int lastEarned = default;
            public int keys = default;


        }


        private const string DataKey = "PlayerData";

        private static Data data;



        public static int TotalStars
        {
            get => data.totalStars;

            private set
            {
                data.totalStars = value;
                SaveUtil.SetObjectValue(DataKey, data);
            }
        }

        public static int Level
        {
            get => data.level;

            private set
            {
                data.level = value;
                SaveUtil.SetObjectValue(DataKey, data);
            }
        }

        public static int Money
        {
            get => data.money;

            private set
            {
                data.money = value;
                SaveUtil.SetObjectValue(DataKey, data);
            }
        }

        public static int LastEarned
        {
            get => data.lastEarned;

            private set
            {
                data.lastEarned = value;
                SaveUtil.SetObjectValue(DataKey, data);
            }
        }

        public static int Keys
        {
            get => data.keys;

            private set
            {
                data.keys = value;
                SaveUtil.SetObjectValue(DataKey, data);
            }
        }



        static PlayerStatistics()
        {
            data = SaveUtil.GetObjectValue<Data>(DataKey);

            if (data == null)
            {
                UnityEngine.Debug.Log("NEW GAME STARTED");
            }

            data = data ?? new Data();
            SaveData();
        }


        public static void UpLevel() =>
            Level++;

        public static void AddKey()
        {
            int curK = Keys;
            curK++;
            if (curK > 3)
            {
                return;
            }



            Keys = curK;
            KeyUpdated.Invoke(Keys);
            UnityEngine.Debug.Log("key++ = " + Keys);
        }

        public static bool RemoveKey()
        {
            int curK = Keys;
            curK--;
            if (curK < 0)
            {
                return false;
            }


            Keys = curK;
            KeyUpdated.Invoke(Keys);
            return true;
        }


        public static void AddMoney(int collectedMoney, Vector3 pos)
        {

            Money += collectedMoney;
            LastEarned = collectedMoney;
            MoneyUpdated.Invoke(collectedMoney, pos);
        }

        public static void RemoveMoney(int _money)
        {
            Money -= _money;
            Money = Money < 0 ? 0 : Money;
            MoneyUpdated.Invoke(-_money, new Vector3());
        }

        public static void AddStars(int count) =>
            TotalStars += count;

        public static void ResetStars() =>
            TotalStars = 0;

        private static void SaveData() =>
            SaveUtil.SetObjectValue(DataKey, data);
    }

}
