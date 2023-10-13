using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private float _maxProtection;
        private IEnemyHealthView _healthView;
        private IAnimatorIK _animatorIK;


        public EnemyDamageSystem(float maxHealth, float maxProtection)
        {

            _maxHealth = maxHealth;
            _maxProtection = maxProtection;
        }


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _healthView = _components.BaseObject.GetComponentInChildren<IEnemyViews>().Health;
            _isDead = _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable.IsDeadFlag;
            _attackable = _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable;
            _animatorIK = _components.BaseObject.GetComponent<IAnimatorIK>();
        }


        protected override void OnEnable()
        {

            _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable.SetMaxHealth(_maxHealth, OnSubscribe);
            _isDead = _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable.IsDeadFlag;
            _isRewardReady = _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.Attackable.IsRewardReadyFlag;
            _healthView.RefreshHealthProtection(_attackable.HealthProtection.Value, _maxProtection);

            _attackable.Health.Subscribe(val =>
            {
                _healthView.RefreshHealth(_attackable.Health.Value, _maxHealth);

            }).AddTo(_disposables);

            _attackable.HealthProtection.Subscribe(val =>
            {
                _healthView.RefreshHealthProtection(_attackable.HealthProtection.Value, _maxProtection);

            }).AddTo(_disposables);


            _healthView.Show();
        }


        protected override void OnDestroy() => Dispose();


        private void OnSubscribe(ReactiveProperty<float> healthProperty)
        {
            Dispose();
            _disposables.Add(healthProperty.Subscribe(OnDamage));
        }


        private void OnDamage(float healthCompleted)
        {

            if(_attackable.HealthProtection.Value > 0)
            {
                return;
            }
            else
            {

                if(_animatorIK != null)
                {
                    _animatorIK.UpdateShieldObject(false);
                }
            }
 
            if (healthCompleted <= 0 )
            {
                    _healthView.Deactivate();
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