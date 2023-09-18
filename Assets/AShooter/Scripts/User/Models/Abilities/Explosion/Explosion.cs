using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


namespace User
{

    public sealed class Explosion : MonoBehaviour, IDisposable
    {

        public float Damage { get; set; }
        public float DamageOverTime { get; set; }
        public float DamageRate { get; set; }
        public float Radius { get; set; }
        public float Force { get; set; }
        public float UpwardsModifier { get; set; }
        public float Lifetime { get; set; }
        public LayerMask LayerMask { get; set; }
        public ParticleSystem Effect { get; set; }
        public float EffectDestroyDelay { get; set; }


        private List<IDisposable> _disposables = new();


        private void Start()
        {
            _disposables.Add(
                Observable
                    .Interval(TimeSpan.FromSeconds(DamageRate))
                    .Subscribe(_ => PerformExplosion(DamageOverTime, false))
            );
        }


        private void OnDestroy()
        {
            Dispose();
            PerformExplosion(Damage, true);
            SpawnEffectOnDestroy();
        }


        private void PerformExplosion(float damage, bool explode)
        {
            if (!transform) return;

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, Radius, LayerMask);

            foreach (Collider hit in colliders)
            {
                if (hit.TryGetComponent(out IAttackable unit))
                {
                    ApplyDamage(unit, damage);
                }

                if (explode)
                {
                    if (hit.TryGetComponent(out Rigidbody unitRB))
                    {
                        unitRB.AddExplosionForce(Force, transform.position, Radius, UpwardsModifier, ForceMode.Impulse);
                    }
                }
            }
        }


        public void ApplyDamage(IAttackable unit, float damage)
        {
            unit.TakeDamage(damage);
        }


        private void SpawnEffectOnDestroy()
        {
            if (Effect)
            {
                var effect = Instantiate(Effect, transform.position, Effect.transform.rotation);
                Destroy(effect.gameObject, EffectDestroyDelay);
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }


        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
        }


    }
}
