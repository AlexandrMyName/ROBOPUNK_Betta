using Abstracts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace User.Presenters
{

    public class StoreView : MonoBehaviour, IStoreView
    {
        [Space(10)]
        [Header("Buttons:")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _passiveSkillsButton;
        [SerializeField] private Button _assistButton;
        [SerializeField] private Button _weaponButton;
        [SerializeField] private Button _armorButton;

        [Space(10)]
        [Header("TradingPanels:")]
        [SerializeField] private GameObject _papassiveSkillsTradingPanels;
        [SerializeField] private GameObject _assistTradingPanels;
        [SerializeField] private GameObject _weaponTradingPanels;
        [SerializeField] private GameObject _armorTradingPanels;

        [Space(10)]
        [Header("PassivePanel:")]
        [SerializeField] private Button _speedButton;
        [SerializeField] private Button _dashButton;
        [SerializeField] private Button _shieldButton;
        [SerializeField] private Button _RateOfFireButton;
        [SerializeField] private Button _maxHealthButton;

        [Space(10)]
        [Header("AssistPanel:")]
        [SerializeField] private Button _firstAidKitButton;


        public void OnClickPassiveSkills()
        {
            _papassiveSkillsTradingPanels.SetActive(true);
            _assistTradingPanels.SetActive(false);
            _weaponTradingPanels.SetActive(false);
            _armorTradingPanels.SetActive(false);
        }


        public void OnClickAssist()
        {
            _papassiveSkillsTradingPanels.SetActive(false);
            _assistTradingPanels.SetActive(true);
            _weaponTradingPanels.SetActive(false);
            _armorTradingPanels.SetActive(false);
        }


        public void OnClickWeapon()
        {
            _papassiveSkillsTradingPanels.SetActive(false);
            _assistTradingPanels.SetActive(false);
            _weaponTradingPanels.SetActive(true);
            _armorTradingPanels.SetActive(false);
        }


        public void OnClickArmor()
        {
            _papassiveSkillsTradingPanels.SetActive(false);
            _assistTradingPanels.SetActive(false);
            _weaponTradingPanels.SetActive(false);
            _armorTradingPanels.SetActive(true);
        }


        public void Show()
        {
            gameObject.SetActive(true);
        }


        public void Hide()
        {
            gameObject.SetActive(false);
        }


        public bool GetActivityState()
        {
            return gameObject.activeSelf;
        }


        public void SetInscriptions(StoreItemConfig storeHealthDataConfig, StoreItemConfig storeSpeedDataConfig, StoreItemConfig storeDamageDataConfig)
        {
        }


        public void SubscribeClickButtons(UnityAction onClickButtonBack)
        {
            _backButton.onClick.AddListener(onClickButtonBack);
        }


    }
}