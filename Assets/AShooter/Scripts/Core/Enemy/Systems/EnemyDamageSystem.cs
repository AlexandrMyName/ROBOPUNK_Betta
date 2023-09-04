using System;
using System.Collections.Generic;
using Abstracts;
using UniRx;

namespace Core
{
    public class EnemyDamageSystem : BaseSystem
    {
        private IGameComponents _components;
        private List<IDisposable> _disposables = new();
        private ReactiveProperty<bool> _isDead;


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _isDead = _components.BaseObject.GetComponent<Enemy>().IsDeadFlag;
        }
        protected override void OnDestroy() => Dispose();

        protected override void OnEnable()
        {
            _components.BaseObject.GetComponent<IAttackable>().SetMaxHealth(GameLoopManager.EnemyMaxHealth, OnSubscribe);
            _isDead = _components.BaseObject.GetComponent<IAttackable>().IsDeadFlag;
        }
        private void OnSubscribe(ReactiveProperty<float> healthProperty)
        {
            Dispose();
            _disposables.Add(healthProperty.Subscribe(OnDamage));
        }
        private void OnDamage(float healthCompleted)
        {
            if (healthCompleted <= 0)
            {
                _isDead.Value = true;
                Dispose();
            }
            else
            {

            }
        }
        private void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }
    }
}


