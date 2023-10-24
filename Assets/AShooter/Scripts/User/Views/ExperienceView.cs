using Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace User.View
{

    public class ExperienceView : MonoBehaviour, IExperienceView
    {

        [SerializeField] private Slider _experienceSlider;
        [SerializeField] private Image _fillImage;
        [SerializeField] private TMP_Text _textCurrentLevelUI;
        [SerializeField] private TMP_Text _textCurrentExperienceUI;
        [SerializeField] private TMP_Text _textProgressExperienceUI;


        public void Show() => gameObject.SetActive(true);


        public void Hide()
        {
            gameObject.SetActive(false);
        }


        public bool GetActivityState()
        {
            if (!this) return false;
            return gameObject.activeSelf;
        }


        public void ChangeDisplay(float valueCurrentExperience, int valueLvl, float valueProgressExperience)
        {
            if (valueLvl.ToString() != _textCurrentLevelUI.text)
            {
                _experienceSlider.minValue = _experienceSlider.maxValue;
            }

            _experienceSlider.maxValue = valueProgressExperience;
            _experienceSlider.value = valueCurrentExperience;

            _textCurrentExperienceUI.text = ((int)valueCurrentExperience).ToString();
            _textProgressExperienceUI.text = ((int)valueProgressExperience).ToString();
            _textCurrentLevelUI.text = valueLvl.ToString();
        }


    }
}