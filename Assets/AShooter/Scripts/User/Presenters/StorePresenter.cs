using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace User.Presenters
{

    public class StorePresenter : MonoBehaviour
    {

        [Inject] private StoreDataConfig _itemConfigs;

        [SerializeField] private Button _healthButton;
        [SerializeField] private Button _speedButton;
        [SerializeField] private Button _damageButton;


        public void Init(   UnityAction<int, float> onClickHealthButton, 
                            UnityAction<int, float> onClickSpeedButton, 
                            UnityAction<int, float> onClickDamageButton)
        {
            _healthButton.onClick.AddListener(() => onClickHealthButton(_itemConfigs.HealthItems.price, _itemConfigs.HealthItems.improvementCoefficient));
            _speedButton.onClick.AddListener(() => onClickSpeedButton(_itemConfigs.SpeedItems.price, _itemConfigs.SpeedItems.improvementCoefficient));
            _damageButton.onClick.AddListener(() => onClickDamageButton(_itemConfigs.DamageItems.price, _itemConfigs.DamageItems.improvementCoefficient));
        }


        private void OnDestroy()
        {
            _healthButton.onClick.RemoveAllListeners();
            _speedButton.onClick.RemoveAllListeners();
            _damageButton.onClick.RemoveAllListeners();
        }


    }
}