using System.Collections.Generic;
using UnityEngine;
using User;


namespace Abstracts
{

    public interface IStoreEnhancement
    {

        public List<StoreItemConfig> PassiveUpgradeItems { get; }

        public List<StoreItemConfig> AssistUpgradeItems { get; }

        public GameObject StoreItemPrefab { get; }

    }
}
