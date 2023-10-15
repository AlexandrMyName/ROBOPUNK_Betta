using Abstracts;
using System.Collections.Generic;
using User;


namespace Core.Components
{

    public sealed class PlayerLevelRewardComponent : ILevelReward
    {

        public List<LevelRewardItemConfig> RewardItems { get; private set; }

        public int NumberOfActiveRewardItems { get; private set; }


        public PlayerLevelRewardComponent(LevelRewardItemsConfigs storeItemsDataConfig)
        {

            RewardItems = storeItemsDataConfig.RewardItemsConfigs;
            NumberOfActiveRewardItems = storeItemsDataConfig.numberOfActiveRewardItems;
        }


    }
}