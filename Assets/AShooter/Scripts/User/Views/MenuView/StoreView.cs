using Abstracts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


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
        [Header("VerticalLayoutGroup:")]
        [SerializeField] private GameObject _passiveSkillsGroupUI;
        [SerializeField] private GameObject _assistItemGroupUI;
        [SerializeField] private GameObject _weaponGroupUI;
        [SerializeField] private GameObject _armorGroupUI;


        public GameObject PassiveSkillsGroupUI { get { return _passiveSkillsGroupUI; } }
        public GameObject AssistItemGroupUI { get { return _assistItemGroupUI; } }
        public GameObject WeaponGroupUI { get { return _weaponGroupUI; } }
        public GameObject ArmorGroupUI { get { return _armorGroupUI; } }


        public void OnClickPassiveSkills()
        {
            _passiveSkillsGroupUI.SetActive(true);
            _assistItemGroupUI.SetActive(false);
            _weaponGroupUI.SetActive(false);
            _armorGroupUI.SetActive(false);
        }


        public void OnClickAssist()
        {
            _passiveSkillsGroupUI.SetActive(false);
            _assistItemGroupUI.SetActive(true);
            _weaponGroupUI.SetActive(false);
            _armorGroupUI.SetActive(false);
        }


        public void OnClickWeapon()
        {
            _passiveSkillsGroupUI.SetActive(false);
            _assistItemGroupUI.SetActive(false);
            _weaponGroupUI.SetActive(true);
            _armorGroupUI.SetActive(false);
        }


        public void OnClickArmor()
        {
            _passiveSkillsGroupUI.SetActive(false);
            _assistItemGroupUI.SetActive(false);
            _weaponGroupUI.SetActive(false);
            _armorGroupUI.SetActive(true);
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


        public void SubscribeClickButtons(UnityAction onClickButtonBack)
        {
            _backButton.onClick.AddListener(onClickButtonBack);
        }


    }
}