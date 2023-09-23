using System;
using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(StoreDataConfig), menuName = "Config/" + nameof(StoreDataConfig))]
    public class StoreDataConfig : ScriptableObject
    {
        public StoreItemConfig HealthItems;
        public StoreItemConfig SpeedItems;
        public StoreItemConfig DamageItems;
    }


    [Serializable]
    public class StoreItemConfig
    {
        public string description;
        public string name;
        public Sprite Icon;
        public int price;
        public float improvementCoefficient;
    }
}