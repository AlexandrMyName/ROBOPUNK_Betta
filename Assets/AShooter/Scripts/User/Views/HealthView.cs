using Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace User.View
{
    
    public class HealthView : MonoBehaviour, IHealthView 
    {

        [SerializeField] private TMP_Text _textUI;
        [SerializeField] private Slider _hP_Slider;
        [SerializeField] private Image _colorReflectionFill;


        public void Show() => gameObject.SetActive(true);


        public void Hide()
        {
            if (this)  
            gameObject.SetActive(false);
        }


        public bool GetActivityState()
        {
            if (!this) return false;
            return gameObject.activeSelf;
        }


        public void ChangeDisplay(float healthValue, float maxValue)
        {

            if (healthValue < 0)
            {
                gameObject.SetActive(false);
                return;
            }

            _textUI.text = $"Health : {healthValue}";
           
            if(_hP_Slider.maxValue != maxValue)
            {
                _hP_Slider.maxValue = maxValue;
            }

            _hP_Slider.value = healthValue;

            var color = Color.Lerp(Color.red, Color.green, healthValue / maxValue);

            _colorReflectionFill.color = color;
        }


    }
}