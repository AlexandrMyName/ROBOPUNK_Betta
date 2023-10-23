using System;
using Abstracts;
using NTC.OverlapSugar;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Core;

namespace User
{

    [RequireComponent(typeof(Rigidbody))]
    public sealed class Projectile : MonoBehaviour, IDisposable
    {

        [SerializeField] private Rigidbody _projectileRigidbody;

        [Header("Explosion")]
        [SerializeField] private OverlapSettings _explosionOverlapSettings;

        [SerializeField] private ProjectileType _projectileType;

        [SerializeField] private float _lifeTime = 3.0f;

        private readonly List<IEnemy> _explosionOverlapResults = new(32);

        private Projectile _collidedProjectile;

        private AudioClip _expolisionAudioClip;


        public bool IsProjectileDisposed { get; private set; }
        public Rigidbody Rigidbody => _projectileRigidbody;
        public float Damage { get; set; }
        public ParticleSystem Effect { get; set; }
        public float EffectDestroyDelay { get; set; }
        public List<IDisposable> _disposables = new();


        private void Awake()
        {
            _expolisionAudioClip = SoundManager.Config.GetSound(SoundType.Damage, SoundModelType.Ability_Expolision);
        }


        private void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(_lifeTime)).Subscribe(_ =>
            {
                if (!IsProjectileDisposed)
                    Destroy(gameObject);
            }).AddTo(_disposables);
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (IsProjectileDisposed) return;
            if (collision.collider.isTrigger) return;
            DisposeProjectile(collision);
        }


        public void DisposeProjectile(Collision collision)
        {
            if (_projectileType == ProjectileType.Rocket)
            {
                PerformExplosion();
                SpawnEffectOnDestroy();
                Destroy(gameObject);
                IsProjectileDisposed = true;
            }

            if (_projectileType == ProjectileType.Bullet && !collision.gameObject.TryGetComponent<Projectile>(out _collidedProjectile))
            {
                Destroy(gameObject);
                IsProjectileDisposed = true;
            }
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

            var effectAudioSource = effect.GetComponent<AudioSource>();
            PlaySound(effectAudioSource, _expolisionAudioClip);

            Destroy(effect.gameObject, EffectDestroyDelay);
        }


        private void PlaySound(AudioSource audioSource, AudioClip audioClip)
        {
            if ((audioSource != null) && (audioClip != null))
                audioSource.PlayOneShot(audioClip);
        }


        private void ApplyDamage(IEnemy unit)
        {
            unit.ComponentsStore.Attackable.TakeDamage(Damage);
        }

        public void Dispose()
        {
            _disposables.ForEach(disp => disp.Dispose());
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
