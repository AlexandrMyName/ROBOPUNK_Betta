using System;
using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(StoreItemsDataConfig), menuName = "Config/" + nameof(StoreItemsDataConfig))]
    public class StoreItemsDataConfig : ScriptableObject
    {
        public StoreItemConfig HealthEnhancement;
        public StoreItemConfig SpeedEnhancement;
        public StoreItemConfig DamageEnhancement;
    }

    [Serializable]
    public class StoreItemConfig
    {
        public string description;
        public string nameItem;
        public Sprite Icon;
        public int price;
        public float improvementCoefficient;
        public string unitImprovementCoefficient;
    }
}