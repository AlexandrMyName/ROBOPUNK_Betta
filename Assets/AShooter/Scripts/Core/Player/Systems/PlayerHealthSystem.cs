using Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


namespace Core
{

    public class PlayerHealthSystem : BaseSystem
    {

        private IGameComponents _components;
        private List<IDisposable> _disposables = new();
        private IAttackable _attackable;


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _attackable = _components.BaseObject.GetComponent<IAttackable>();
            _disposables.Add(_attackable.Health.Subscribe(OnDamage));
        }


        protected override void OnDestroy()
        {
            _disposables.ForEach(d => d.Dispose());
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
                _components.BaseObject.gameObject.SetActive(false);
            }
        }


    }
}

