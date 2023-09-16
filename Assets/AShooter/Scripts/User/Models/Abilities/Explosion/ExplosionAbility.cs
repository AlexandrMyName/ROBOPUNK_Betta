using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


namespace User
{

    public sealed class ExplosionAbility : IDisposable
    {

        public Explosion ExplosionObject { get; private set; }

        public float Damage { get; private set; }

        public int Radius { get; private set; }

        public float Force { get; set; }

        public float Lifetime { get; private set; }

        public float UsageTimeout { get; private set; }

        public LayerMask LayerMask { get; private set; }

        public ParticleSystem Effect { get; private set; }

        public float EffectDestroyDelay { get; private set; }

        public bool IsReady { get; private set; }


        private List<IDisposable> _disposables = new();


        public ExplosionAbility(Explosion explosionObject, float damage, int radius, float force, float lifetime,
            float usageTimeout, LayerMask layerMask, ParticleSystem effect, float effectDestroyDelay)
        {
            ExplosionObject = explosionObject;
            Damage = damage;
            Radius = radius;
            Force = force;
            Lifetime = lifetime;
            UsageTimeout = usageTimeout;
            LayerMask = layerMask;
            Effect = effect;
            EffectDestroyDelay = effectDestroyDelay;
            IsReady = true;
        }


        public void Apply(Transform playerTransform)
        {
            InstantiateExplosion(playerTransform);

            IsReady = false;
            Cooldown();
        }


        private void InstantiateExplosion(Transform playerTransform)
        {
            var playerPosition = playerTransform.position;
            var spawnPoint = new Vector3(playerPosition.x, 0.2f, playerPosition.z);

            var explosion = GameObject.Instantiate(ExplosionObject, spawnPoint, Quaternion.identity);
            explosion.Damage = Damage;
            explosion.Radius = Radius;
            explosion.Force = Force;
            explosion.LayerMask = LayerMask;
            explosion.Effect = Effect;
            explosion.EffectDestroyDelay = EffectDestroyDelay;

            GameObject.Destroy(explosion.gameObject, Lifetime);
        }


        public void Cooldown()
        {
            if (!IsReady)
            {
                _disposables.Add(
                    Observable
                        .Timer(TimeSpan.FromSeconds(UsageTimeout))
                        .Subscribe(_ => IsReady = true)
                );
            }
        }

        
        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
        }


    }
}
