using UnityEngine;
using Abstracts;
using UnityEngine.UI;


namespace User.View
{

    public class EnemyHealthView : MonoBehaviour, IEnemyHealthView
    {

        [SerializeField] private Slider _healthSlider;


        public void Deactivate() => gameObject.SetActive(false);


        public void RefreshHealth(float currentHealth, float maxHealth)
        {

            if(_healthSlider.maxValue != maxHealth)
            {
                _healthSlider.maxValue = maxHealth;
            }
            _healthSlider.value = currentHealth;
        }


        private void Update()
        {
            //Rotate to camera
        }


        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}