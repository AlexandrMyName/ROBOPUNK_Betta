using Abstracts;
using UnityEngine;
using UnityEngine.UI;


namespace User.View
{

    public class ShieldView : MonoBehaviour, IShieldView
    {

        [SerializeField] private Slider _timeSlider;
        [SerializeField] private Slider _protectionSlider;


        private void Awake() => gameObject.SetActive(false);


        public void Deactivate()
        {

            _timeSlider.maxValue = 0;
            _protectionSlider.maxValue = 0;
            gameObject.SetActive(false);
        }


        public void RefreshProtection(float currentProtection, float maxProtection)
        {

            if (_protectionSlider.maxValue != maxProtection)
            {
                _protectionSlider.maxValue = maxProtection;
                _protectionSlider.value = currentProtection;
            }
            _protectionSlider.value = currentProtection;
        }


        public void RefreshTime(float maxTime)
        {

            if (_timeSlider.maxValue != maxTime)
            {
                _timeSlider.maxValue = maxTime;
                _timeSlider.value = maxTime;
            }
            _timeSlider.value -= Time.deltaTime;
        }


        public void Show() => gameObject.SetActive(true);

    }

}