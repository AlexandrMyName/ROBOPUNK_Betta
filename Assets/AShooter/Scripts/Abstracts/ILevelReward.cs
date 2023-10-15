using System.Collections.Generic;
using UnityEngine;
using User;


namespace Abstracts
{

    public interface ILevelReward
    {

        public List<LevelRewardItemConfig> RewardItems { get; }

        public int NumberOfActiveRewardItems { get; }

    }
} 