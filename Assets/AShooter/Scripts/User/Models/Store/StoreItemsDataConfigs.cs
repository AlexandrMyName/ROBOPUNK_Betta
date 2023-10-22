using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace User
{

    [CreateAssetMenu(fileName = nameof(StoreItemsDataConfigs), menuName = "Config/" + nameof(StoreItemsDataConfigs))]
    public class StoreItemsDataConfigs : ScriptableObject
    {
        public List<StoreItemConfig> PassiveUpgradeItemsConfigs;
        public List<StoreItemConfig> AssistUpgradeItemsConfigs;
        public List<StoreWeaponConfig> WeaponItemsConfigs;
        public List<StoreWeaponConfig> ArmorItemsConfigs;

        public GameObject StoreItemPrefab;
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
        public StoreItemType ItemType;
    }

    
    [Serializable]
    public class StoreWeaponConfig
    {
        public int id;
        public string description;
        public string nameItem;
        public Sprite Icon;
        public int price;
        public StoreItemType ItemType;
    }

    
    [Serializable]
    public class StoreArmorConfig
    {
        public int id;
        public string description;
        public string nameItem;
        public Sprite Icon;
        public int price;
        public StoreItemType ItemType;
    }
    
}