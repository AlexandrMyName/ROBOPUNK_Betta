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
        private ReactiveProperty<bool> _isRewardReady;
        private float _maxHealth;


        public EnemyDamageSystem(float maxHealth)
        {
            _maxHealth = maxHealth;
        }


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _isDead = _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable.IsDeadFlag;
        }


        protected override void OnEnable()
        {
            _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable.SetMaxHealth(_maxHealth, OnSubscribe);
            _isDead = _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable.IsDeadFlag;
            _isRewardReady = _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable.IsRewardReadyFlag;
        }


        protected override void OnDestroy() => Dispose();


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
                _isRewardReady.Value = true;
                Dispose();
            }
        }


        private void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }


    }
}