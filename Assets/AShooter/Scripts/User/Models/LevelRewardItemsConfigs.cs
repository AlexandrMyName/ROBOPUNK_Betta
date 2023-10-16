using System;
using System.Collections.Generic;
using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(LevelRewardItemsConfigs), menuName = "Config/" + nameof(LevelRewardItemsConfigs))]
    public class LevelRewardItemsConfigs : ScriptableObject
    {
        public List<LevelRewardItemConfig> RewardItemsConfigs;

        public int numberOfActiveRewardItems;
    }


    [Serializable]
    public class LevelRewardItemConfig
    {
        public int id;    
        public string description;
        public string nameItem;
        public Sprite Icon;
        public float upgradeCoefficient;
        public string unitImprovementCoefficient;
        public GameObject effectPrefrab;
    }


}