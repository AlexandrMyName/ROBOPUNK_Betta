using System;
using System.Collections.Generic;
using Abstracts;
using UniRx;
using UnityEngine;


namespace Core
{
    
    public class EnemyDamageSystem : BaseSystem
    {
        
        private IGameComponents _components;
        private List<IDisposable> _disposables = new();
        

        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _disposables.Add(
                _components.BaseObject.GetComponent<IAttackable>().Health.Subscribe(OnDamage)
                );
        }


        protected override void OnDestroy()
        {
            _disposables.ForEach(d=> d.Dispose());
        }


        private void OnDamage(float amountHealth)
        {
            Debug.LogWarning($"{_components.BaseObject.name} getting damage |DamageSystem|");
        }
        
        
    }
}


