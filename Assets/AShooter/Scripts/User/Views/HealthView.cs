using Abstracts;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace User.View
{
    
    public class HealthView : MonoBehaviour // Test View
    {

        [SerializeField] private TMP_Text _textUI;
        [Inject(Id = "PlayerComponents")] private IComponentsStore _componentsPlayer;


        private void Awake()
        {
            _componentsPlayer.Attackable.Health.Subscribe(ChangeHealth);
        }


        private void ChangeHealth(float healthValue)
        {
            _textUI.text = $"Health : {healthValue}";
        }


    }
}