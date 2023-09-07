using Abstracts;
using NTC.OverlapSugar;
using System.Collections.Generic;
using UnityEngine;


namespace User
{

    [RequireComponent(typeof(Rigidbody))]
    public sealed class Projectile : MonoBehaviour
    {

        [SerializeField] private Rigidbody _projectileRigidbody;

        [Header("Explosion")]
        [SerializeField] private OverlapSettings _explosionOverlapSettings;

        private readonly List<IAttackable> _explosionOverlapResults = new(32);


        public bool IsProjectileDisposed { get; private set; }
        public Rigidbody Rigidbody => _projectileRigidbody;
        public float Damage { get; set; }
        public ParticleSystem Effect { get; set; }
        public float EffectDestroyDelay { get; set; }


        private void OnCollisionEnter(Collision collision)
        {
            if (IsProjectileDisposed) return;

            DisposeProjectile();
        }


        public void DisposeProjectile()
        {
            PerformExplosion();
            SpawnEffectOnDestroy();
            Destroy(gameObject);

            IsProjectileDisposed = true;
        }
        

        private void PerformExplosion()
        {
            if (_explosionOverlapSettings.TryFind(_explosionOverlapResults))
            {
                _explosionOverlapResults.ForEach(ApplyDamage);
            }
        }


        private void SpawnEffectOnDestroy()
        {
            var effect = Instantiate(Effect, transform.position, Effect.transform.rotation);
            Destroy(effect.gameObject, EffectDestroyDelay);
        }


        private void ApplyDamage(IAttackable unit)
        {
            unit.TakeDamage(Damage);
        }


    }
}
