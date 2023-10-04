using System;
using System.Collections.Generic;
using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(StoreItemsDataConfigs), menuName = "Config/" + nameof(StoreItemsDataConfigs))]
    public class StoreItemsDataConfigs : ScriptableObject
    {
        public List<StoreItemConfig> PassiveUpgradeItemsConfigs;
        public List<StoreItemConfig> AssistUpgradeItemsConfigs;
        public List<StoreWeaponConfig> WeaponItemsConfigs;
        public List<StoreWeaponConfig> ArmorItemsConfigs;
    }

    [Serializable]
    public class StoreItemConfig
    {
        public int id;    
        public string description;
        public string nameItem;
        public Sprite Icon;
        public int price;
        public float upgradeCoefficient;
        public string unitImprovementCoefficient;
    }

    [Serializable]
    public class StoreWeaponConfig
    {
        public int id;
        public string description;
        public string nameItem;
        public Sprite Icon;
        public int price;
    }

    [Serializable]
    public class StoreArmorConfig
    {
        public int id;
        public string description;
        public string nameItem;
        public Sprite Icon;
        public int price;
    }
}