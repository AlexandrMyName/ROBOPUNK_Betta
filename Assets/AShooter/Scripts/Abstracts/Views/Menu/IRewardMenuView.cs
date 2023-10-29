using UnityEngine;
using UnityEngine.UI;


namespace Abstracts
{

    public interface IRewardMenuView : IView
    {

        public HorizontalLayoutGroup HorizontalLayoutGroup { get; }

        public GameObject LevelRewardItemPrefab { get; }

        public Animation Animation { get; }

    }

}