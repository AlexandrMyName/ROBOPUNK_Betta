using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Abstracts;


namespace User
{

    public sealed class ExplosionAbility : IDisposable, IAbility
    {

        public Explosion ExplosionObject { get; private set; }

        public Sprite ExplosionIcon { get; private set; }

        public AbilityType AbilityType { get; private set; }

        public float Damage { get; private set; }

        public float DamageOverTime { get; private set; }

        public float DamageRate { get; private set; }

        public int Radius { get; private set; }

        public float Force { get; set; }

        public float UpwardsModifier { get; set; }

        public float Lifetime { get; private set; }

        public float UsageTimeout { get; private set; }

        public LayerMask LayerMask { get; private set; }

        public ParticleSystem DamageOverTimeEffect { get; private set; }

        public int EffectNumPerTick { get; private set; }

        public ParticleSystem Effect { get; private set; }

        public float EffectDestroyDelay { get; private set; }

        public ReactiveProperty<bool> IsReady { get; private set; }


        private List<IDisposable> _disposables = new();


        public ExplosionAbility(Explosion explosionObject, Sprite explosionIcon, AbilityType abilityType, float damage, float damageOverTime, 
            float damageRate, int radius, float force, float upwardsModifier, float lifetime, float usageTimeout, LayerMask layerMask, 
            ParticleSystem damageOverTimeEffect, int effectNumPerTick, ParticleSystem effect, float effectDestroyDelay)
        {
            ExplosionObject = explosionObject;
            ExplosionIcon = explosionIcon;
            AbilityType = abilityType;
            Damage = damage;
            DamageOverTime = damageOverTime;
            DamageRate = damageRate;
            Radius = radius;
            Force = force;
            UpwardsModifier = upwardsModifier;
            Lifetime = lifetime;
            UsageTimeout = usageTimeout;
            LayerMask = layerMask;
            DamageOverTimeEffect = damageOverTimeEffect;
            EffectNumPerTick = effectNumPerTick;
            Effect = effect;
            EffectDestroyDelay = effectDestroyDelay;
            IsReady = new ReactiveProperty<bool>(true);
        }


        public void Apply(Transform playerTransform)
        {
            InstantiateExplosion(playerTransform);

            IsReady.Value = false;
            Cooldown();
        }


        private void InstantiateExplosion(Transform playerTransform)
        {
            var playerPosition = playerTransform.position;
            var spawnPoint = new Vector3(playerPosition.x, 0.2f, playerPosition.z);

            var explosion = GameObject.Instantiate(ExplosionObject, spawnPoint, Quaternion.identity);
            explosion.Ability = this;

            GameObject.Destroy(explosion.gameObject, Lifetime);
        }


        public void Cooldown()
        {
            if (!IsReady.Value)
            {
                _disposables.Add(
                    Observable
                        .Timer(TimeSpan.FromSeconds(UsageTimeout))
                        .Subscribe(_ => IsReady.Value = true)
                );
            }
        }


        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
        }


    }
}
