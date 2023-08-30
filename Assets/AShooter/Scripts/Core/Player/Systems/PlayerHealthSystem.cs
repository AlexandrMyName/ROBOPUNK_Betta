using Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ModestTree;


namespace Core
{

    public class PlayerHealthSystem : BaseSystem
    {

        private IGameComponents _components;
        private List<IDisposable> _disposables = new();


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _disposables.Add(
                _components.BaseObject.GetComponent<IAttackable>().Health.Subscribe(OnDamage)
                );
            DeathCheck();
        }


        protected override void OnDestroy()
        {
            _disposables.ForEach(d => d.Dispose());
        }


        private void OnDamage(float amountHealth)
        {
            Debug.LogWarning($"{_components.BaseObject.name} getting damage |DamageSystem|");  
        }


        private void DeathCheck()
        {
            if (_components.BaseObject.GetComponent<IAttackable>().Health.Value <= 0)
            {
                _components.BaseObject.gameObject.SetActive(false);
            }
        }
    }
}

