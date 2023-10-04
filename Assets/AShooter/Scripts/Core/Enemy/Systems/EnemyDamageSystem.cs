using System;
using System.Collections.Generic;
using Abstracts;
using UniRx;


namespace Core
{

    public class EnemyDamageSystem : BaseSystem
    {

        private IGameComponents _components;
        private IEnemyAttackable _attackable;
        private List<IDisposable> _disposables = new();
        private ReactiveProperty<bool> _isDead;
        private ReactiveProperty<bool> _isRewardReady;
        private float _maxHealth;
        private IEnemyHealthView _healthView;

        public EnemyDamageSystem(float maxHealth)
        {
            _maxHealth = maxHealth;
        }


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _healthView = _components.BaseObject.GetComponentInChildren<IEnemyViews>().Health;
            _isDead = _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable.IsDeadFlag;
            _attackable = _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable;
             
        }


        protected override void OnEnable()
        {
            _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable.SetMaxHealth(_maxHealth, OnSubscribe);
            _isDead = _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable.IsDeadFlag;
            _isRewardReady = _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable.IsRewardReadyFlag;
            _attackable.Health.Subscribe(val =>
            {
                _healthView.RefreshHealth(_attackable.Health.Value, _maxHealth);
            }).AddTo(_disposables);
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
 
                if (_components.Animator != null)
                    _components.Animator
                        .ActivateDeathAnimation(
                        _attackable.CachedHitDamage,
                        _attackable.CachedDirectionDamage);
                else
                {
                    _isDead.Value = true;
                }
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