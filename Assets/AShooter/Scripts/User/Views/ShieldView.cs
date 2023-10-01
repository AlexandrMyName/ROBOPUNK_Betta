using Abstracts;
using UnityEngine;
using UnityEngine.UI;


namespace User.View
{

    public class ShieldView : MonoBehaviour, IShieldView
    {

        [SerializeField] private Slider _protectionSlider;

        public void Deactivate()
        {

            //_protectionSlider.maxValue = 0;
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

        public void Show() => gameObject.SetActive(true);

    }

}