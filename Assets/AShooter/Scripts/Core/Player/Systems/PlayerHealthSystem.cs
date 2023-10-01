using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


namespace Core
{

    public class PlayerHealthSystem : BaseSystem, IDisposable
    {

        public IComponentsStore ComponentsStore { get; private set; }
        private IGameComponents _components;
        private IAttackable _attackable;
        private IPlayerHP _playerHP;
        private IDeathView _loseView;
        private IHealthView _healthView;
        private List<IDisposable> _disposables = new();
         


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _attackable = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Attackable;
            _playerHP = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.PlayerHP;

            _healthView = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.HealthView;
            var currentHealth = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Attackable.Health;
            _disposables.Add(_attackable.Health.Subscribe(UpdateDisplay));
            _healthView.Show();
            _healthView.ChangeDisplay(currentHealth.Value,100);

            _loseView = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.Death;
            _disposables.Add(_attackable.Health.Subscribe(DeathCheck));
        }


        public void Dispose()
        {

            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }


        private void UpdateDisplay(float healthValue) => _healthView.ChangeDisplay(healthValue, 100);//Add max health value 


        private void DeathCheck(float leftHealth)
        {

            if (leftHealth <= 0)
            {

                var playerRigidbody = _components.BaseObject.GetComponent<Rigidbody>();

                playerRigidbody.AddForce(Vector3.back * _playerHP.PunchForce, ForceMode.Impulse);
               
                _playerHP.IsAlive.Value = false;

                InputManager.DisableSystem();

                _loseView.Show();

            }
           
        }

    }
}

