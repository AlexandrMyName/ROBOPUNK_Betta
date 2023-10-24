using Abstracts;
using UnityEngine;
using UnityEngine.UI;


namespace User.Presenters
{

    public class LevelRewardMenuView : MonoBehaviour, IRewardMenuView
    {

        [SerializeField] private HorizontalLayoutGroup _rewardButtons;

        [SerializeField] private GameObject _levelRewardItemPrefab;

        public HorizontalLayoutGroup HorizontalLayoutGroup { get { return _rewardButtons; } }

        public GameObject LevelRewardItemPrefab { get { return _levelRewardItemPrefab; } }


        public bool GetActivityState()
        {
            if (!this) return false;
            return gameObject.activeSelf;
        }


        public void Show()
        {
            gameObject.SetActive(true);
        }


        public void Hide()
        {
            if(this)
            gameObject.SetActive(false);
        }


    }
}