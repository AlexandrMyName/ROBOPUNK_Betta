using UnityEngine;
using Abstracts;
using UnityEngine.UI;


namespace User.View
{

    public class EnemyHealthView : MonoBehaviour, IEnemyHealthView
    {

        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Image _fillImageHealth;

        [SerializeField] private Slider _healthProtectionSlider;
        [SerializeField] private Image _fillImageProtection;

        public void Deactivate() => gameObject.SetActive(false);


        public void RefreshHealth(float currentHealth, float maxHealth)
        {

            if(_healthSlider.maxValue != maxHealth)
            {
                _healthSlider.maxValue = maxHealth;
            }
            _healthSlider.value = currentHealth;

            Color color = Color.Lerp(Color.red, Color.green, currentHealth/maxHealth);
            _fillImageHealth.color = color;

        }


        public void Show() => gameObject.SetActive(true);
        
      
        public void Hide()
        {
            gameObject.SetActive(false);
        }


        public bool GetActivityState()
        {
            return gameObject.activeSelf;
        }

        public void RefreshHealthProtection(float currentProtection, float maxProtection)
        {

            if(currentProtection <= 0)
            {
                _healthProtectionSlider.gameObject.SetActive(false);
                return;
            }

            if (_healthProtectionSlider.maxValue != maxProtection)
            {
                _healthProtectionSlider.maxValue = maxProtection;
            }
            _healthProtectionSlider.value = currentProtection;

            Color color = Color.Lerp(Color.red, Color.green, currentProtection / maxProtection);
            _fillImageProtection.color = color;

        }
    }
}