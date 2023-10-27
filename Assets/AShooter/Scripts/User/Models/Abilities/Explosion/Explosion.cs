﻿using Abstracts;
using Core;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;


namespace User
{

    public sealed class Explosion : MonoBehaviour, IDisposable
    {

        public ExplosionAbility Ability {  get; set; }

        private List<IDisposable> _disposables = new();
        private AudioClip _fireAudioClip;
        private AudioClip _expolisionAudioClip;
        private AudioSource _audioSource;


        private void Start()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            _fireAudioClip = SoundManager.Config.GetSound(SoundType.DamageOverTime, SoundModelType.Ability_Expolision);
            _expolisionAudioClip = SoundManager.Config.GetSound(SoundType.Damage, SoundModelType.Ability_Expolision);

            _disposables.Add(
                Observable
                    .Interval(TimeSpan.FromSeconds(Ability.DamageRate))
                    .Subscribe(_ => PerformExplosion(Ability.DamageOverTime, false))
            );
        }


        private void OnDestroy()
        {
            Dispose();
            PerformExplosion(Ability.Damage, true);
            SpawnEffectOnDestroy();
        }


        private void PerformExplosion(float damage, bool explode)
        {
            if (!transform) return;

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, Ability.Radius, Ability.LayerMask);

            foreach (Collider hit in colliders)
            {
                if (hit.TryGetComponent(out IEnemy unit))
                {
                    ApplyDamage(unit, damage);
                }

                if (explode)
                {
                    if (hit.TryGetComponent(out Rigidbody unitRB))
                        unitRB.AddExplosionForce(Ability.Force, transform.position, Ability.Radius, Ability.UpwardsModifier, ForceMode.Impulse);
                }
            }

            if (!explode)
            {
                SpawnDamageOverTimeEffect();
                PlaySound(_audioSource, _fireAudioClip);
            }
        }


        public void ApplyDamage(IEnemy unit, float damage)
        {
            unit.ComponentsStore.Attackable.TakeDamage(damage);
        }


        private void PlaySound(AudioSource audioSource, AudioClip audioClip)
        {
            if ((audioSource != null) && (audioClip != null))
                audioSource.PlayOneShot(audioClip);
        }


        private void SpawnDamageOverTimeEffect()
        {
            if (Ability.DamageOverTimeEffect)
            {
                for (int i = 0; i < Ability.EffectNumPerTick; i++)
                {
                    Vector3 randomPos = Random.insideUnitSphere * Ability.Radius;
                    var effect = Instantiate(
                        Ability.DamageOverTimeEffect,
                        transform.position + new Vector3(randomPos.x, 0.2f, randomPos.y),
                        Ability.DamageOverTimeEffect.transform.rotation);

                    Destroy(effect.gameObject, Ability.EffectDestroyDelay);
                }
            }
        }


        private void SpawnEffectOnDestroy()
        {
            if (Ability.Effect)
            {
                var effect = Instantiate(Ability.Effect, transform.position, Ability.Effect.transform.rotation);
                var effectAudioSource = effect.GetComponent<AudioSource>();
                PlaySound(effectAudioSource, _expolisionAudioClip);
                Destroy(effect.gameObject, Ability.EffectDestroyDelay);
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Ability.Radius);
        }


        public void Dispose() => _disposables.ForEach(d => d.Dispose());


    }
}
