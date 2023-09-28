using Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace User.View
{
    
    public class HealthView : MonoBehaviour, IHealthView // Test View
    {

        [SerializeField] private TMP_Text _textUI;
        [SerializeField] private Image _HP_Bar;


        public void Show() => gameObject.SetActive(true);

        public void ChangeDisplay(float healthValue)
        {
            _textUI.text = $"Health : {healthValue}";
            _HP_Bar.fillAmount = healthValue / 100;
            
        }


    }
}