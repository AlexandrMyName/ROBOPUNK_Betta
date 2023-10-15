﻿using Abstracts;
using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;
using UniRx;
using User;


namespace Core
{

    public class ExplosionAbilitySystem : BaseSystem, IDisposable
    {

        [Inject] private IInput _input;
        [Inject] private readonly ExplosionAbilityConfig _explosionAbilityConfigs;
        [Inject] private WeaponAbilityPresenter _weaponAbilityPresenter;

        private IGameComponents _components;
        private List<IDisposable> _disposables = new();
        private ExplosionAbility _explosionAbility;
        private Transform _playerTransform;


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _playerTransform = _components.BaseTransform;
        }


        protected override void Start()
        {
            InitializeExplosionAbility(_explosionAbilityConfigs);

            _disposables.AddRange(new List<IDisposable>{
                    _input.Explosion.AxisOnChange.Subscribe(_ => ProcessExplosion(_explosionAbility, _playerTransform))
            });

             _weaponAbilityPresenter.SubscribeAbility(_explosionAbility);
        }


        private void InitializeExplosionAbility(ExplosionAbilityConfig config)
        {
            _explosionAbility = new ExplosionAbility(
                config.ExplosionObject,
                config.ExplosionIcon,
                config.AbilityType,
                config.Damage,
                config.DamageOverTime,
                config.DamageRate,
                config.Radius,
                config.Force,
                config.UpwardsModifier,
                config.Lifetime,
                config.UsageTimeout,
                config.LayerMask,
                config.DamageOverTimeEffect,
                config.EffectNumPerTick,
                config.Effect,
                config.EffectDestroyDelay);
        }


        private void ProcessExplosion(ExplosionAbility explosionAbility, Transform playerTransform)
        {
            if (explosionAbility.IsReady.Value)
                explosionAbility.Apply(playerTransform);
        }


        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }


    }
}
