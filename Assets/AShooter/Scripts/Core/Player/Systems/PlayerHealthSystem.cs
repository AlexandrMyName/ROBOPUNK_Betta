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
        private List<IDisposable> _disposables = new();
         


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _attackable = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Attackable;
            _playerHP = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.PlayerHP;
            _disposables.Add(_attackable.Health.Subscribe(DeathCheck));
        }


        public void Dispose()
        {

            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }
 

        private void DeathCheck(float leftHealth)
        {

            if (leftHealth <= 0)
            {
                var playerRigidbody = _components.BaseObject.GetComponent<Rigidbody>();

                playerRigidbody.AddForce(Vector3.back * _playerHP.PunchForce, ForceMode.Impulse);
               
                _playerHP.IsAlive.Value = false;

                InputManager.DisableSystem();
            }
            else
            {
                Debug.LogWarning($"{_components.BaseObject.name} getting damage |DamageSystem|");
            }
        }

    }
}

