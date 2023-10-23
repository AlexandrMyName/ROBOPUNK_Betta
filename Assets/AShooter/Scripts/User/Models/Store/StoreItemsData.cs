using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using User.Presenters;


namespace User
{

    [CreateAssetMenu(fileName = nameof(StoreItemsData), menuName = "Config/" + nameof(StoreItemsData))]
    public class StoreItemsData : ScriptableObject
    {
        public List<StoreItemConfig> PassiveUpgradesData;
        public List<StoreItemConfig> WeaponsData;
        public List<StoreItemConfig> SkinsData;

        public StoreItemView StoreItemPrefab;
    }

    
    [Serializable]
    public struct StoreItemConfig
    {
        public int Id;    
        public string Description;
        public string NameItem;
        public Sprite Icon;
        public int Price;
        public float UpgradeCoefficient;
        public StoreItemType ItemType;
    }
    
    
}