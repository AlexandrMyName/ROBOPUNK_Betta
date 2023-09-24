using Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


namespace Core
{

    public class PlayerHealthSystem : BaseSystem, IDisposable
    {
        public IComponentsStore ComponentsStore { get; private set; }

        private IGameComponents _components;
        private List<IDisposable> _disposables = new();
        private IAttackable _attackable;
        private IPlayerHP _playerHP;


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _attackable = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Attackable;
            _playerHP = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.PlayerHP;
            _disposables.Add(_attackable.Health.Subscribe(OnDamage));
        }


        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }


        private void OnDamage(float leftHealth)
        {
            Debug.LogWarning($"{_components.BaseObject.name} getting damage |DamageSystem|");
            DeathCheck(leftHealth);
        }


        private void DeathCheck(float leftHealth)
        {
            if (leftHealth <= 0)
            {
                var playerRigidbody = _components.BaseObject.GetComponent<Rigidbody>();
                playerRigidbody.AddForce(Vector3.back * _playerHP._deathPunchForce, ForceMode.Impulse);

                _playerHP._playerAlive = false;
            }
        }


    }
}

