using Abstracts;
using UnityEngine;
using UnityEngine.UI;


namespace User.View
{

    public class ShieldView : MonoBehaviour, IShieldView
    {

        [SerializeField] private Slider _slider;

        public void Deactivate()
        {

            _slider.maxValue = 0;
            gameObject.SetActive(false);
        }


        public void Refresh(float maxTime)
        {

            if (_slider.maxValue != maxTime)
            {
                _slider.maxValue = maxTime;
                _slider.value = maxTime;
            }
            _slider.value -= Time.deltaTime;
        }


        public void Show() => gameObject.SetActive(true);

    }

}