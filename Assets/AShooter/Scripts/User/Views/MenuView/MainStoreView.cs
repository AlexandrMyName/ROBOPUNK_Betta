using Abstracts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


namespace User.Presenters
{

    public sealed class MainStoreView : MonoBehaviour, IMainStoreView
    {

        [Space(10)]
        [Header("Buttons:")]
        private string _buttonsHeader;
        [field: SerializeField] public Button BackButton { get; private set; }
        [field: SerializeField] public Button PassiveSkillsButton { get; private set; }
        [field: SerializeField] public Button WeaponsButton { get; private set; }
        [field: SerializeField] public Button SkinsButton { get; private set; }

        [Space(10)]
        [Header("View Port Layouts:")]
        private string _layoutsHeader;
        [field: SerializeField] public GameObject PassiveSkillsGroupUI { get; private set; }
        [field: SerializeField] public GameObject PassiveSkillsContent { get; private set; }
        [field: SerializeField] public GameObject WeaponsGroupUI { get; private set; }
        [field: SerializeField] public GameObject WeaponsContent { get; private set; }
        
        [field: SerializeField] public GameObject SkinsGroupUI { get; private set; }
        [field: SerializeField] public GameObject SkinsContent { get; private set; }

        [field: SerializeField] public TMP_Text MetaExperienceValue { get; private set; }
        [field: SerializeField] public TMP_Text GoldValue { get; private set; }


        public void OnClickPassiveSkills()
        {
            PassiveSkillsGroupUI.SetActive(true);
            WeaponsGroupUI.SetActive(false);
            SkinsGroupUI.SetActive(false);
        }


        public void OnClickWeapons()
        {
            PassiveSkillsGroupUI.SetActive(false);
            WeaponsGroupUI.SetActive(true);
            SkinsGroupUI.SetActive(false);
        }


        public void OnClickSkins()
        {
            PassiveSkillsGroupUI.SetActive(false);
            WeaponsGroupUI.SetActive(false);
            SkinsGroupUI.SetActive(true);
        }


        public void Show()
        {
            gameObject.SetActive(true);
        }


        public void Hide()
        {
            gameObject.SetActive(false);
        }

        
        public bool GetActivityState() => gameObject.activeSelf;
        


    }
}