using Abstracts;
using System.Collections.Generic;
using UnityEngine;
using User;


namespace Core.Components
{

    public sealed class PlayerStoreEnhancementComponent : IStoreEnhancement
    {

        public List<StoreItemConfig> PassiveUpgradeItems { get; private set; }

        public List<StoreItemConfig> AssistUpgradeItems { get; private set; }

        public GameObject StoreItemPrefab { get; private set; }


        public PlayerStoreEnhancementComponent(StoreItemsDataConfigs storeItemsDataConfig)
        {
            PassiveUpgradeItems = storeItemsDataConfig.PassiveUpgradeItemsConfigs;
            AssistUpgradeItems = storeItemsDataConfig.AssistUpgradeItemsConfigs;

            StoreItemPrefab = storeItemsDataConfig.StoreItemPrefab;
        }


    }
}