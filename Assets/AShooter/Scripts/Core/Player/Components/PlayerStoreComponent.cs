using Abstracts;
using System.Collections.Generic;
using User;


namespace Core.Components
{

    public sealed class PlayerStoreEnhancementComponent : IStoreEnhancement
    {

        public List<StoreItemConfig> PassiveUpgradeItems { get; private set; }

        public List<StoreItemConfig> AssistUpgradeItems { get; private set; }


        public PlayerStoreEnhancementComponent(StoreItemsDataConfigs storeItemsDataConfig)
        {
            PassiveUpgradeItems = new List<StoreItemConfig>();
            PassiveUpgradeItems = storeItemsDataConfig.PassiveUpgradeItemsConfigs;

            AssistUpgradeItems = new List<StoreItemConfig>();
            AssistUpgradeItems = storeItemsDataConfig.AssistUpgradeItemsConfigs;
        }


    }
}