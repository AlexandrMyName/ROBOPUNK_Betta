using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Abstracts;
using UniRx;
using UnityEngine;
using User;
using static UnityEditor.Progress;
using UnityEngine;

namespace Core
{

    public class EnemyDamageSystem : BaseSystem, IDisposable
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
        private IWeaponStorage _weaponStorage;
        private AudioSource _audioSource;
        private AudioClip _deathAudioClip;


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
            _weaponStorage = _components.BaseObject.GetComponent<IEnemy>().ComponentsStore.WeaponStorage;
        }


        protected override void OnEnable()
        {

            _audioSource = _components.BaseObject.GetComponent<AudioSource>();
            _deathAudioClip = SoundManager.Config.GetSound(SoundType.Death, SoundModelType.Enemy);

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

                PlaySound(_audioSource, _deathAudioClip);

                _isRewardReady.Value = true;
                Dispose();

                GetPickUpItem();
            }
        }


        private void PlaySound(AudioSource audioSource, AudioClip audioClip)
        {
            if ((audioSource != null) && (audioClip != null))
                audioSource.PlayOneShot(audioClip);
        }


        private void GetPickUpItem()
        {
            float random = UnityEngine.Random.Range(0f, 1f);
            float dropProbability = 0.4f;
            var weaponTypes = new HashSet<WeaponType>() { WeaponType.Shotgun, WeaponType.Rifle, WeaponType.RocketLauncher };

            var weaponConfigs = _weaponStorage.WeaponConfigs
                .Where(weaponConfig => weaponTypes.Contains(weaponConfig.WeaponType))
                .ToList();
            var configIndex = UnityEngine.Random.Range(0, weaponConfigs.Count);
            var config = weaponConfigs[configIndex];
            var pickUpItemTypeIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(PickUpItemType)).Length);

            PickUpItemModel pickUpItemModel = new PickUpItemModel(config, (PickUpItemType)pickUpItemTypeIndex, _components.BaseObject.transform.position);

            if (random <= dropProbability)
                _weaponStorage.GetPickUpItem(pickUpItemModel);
        }


        
        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }
    }
}