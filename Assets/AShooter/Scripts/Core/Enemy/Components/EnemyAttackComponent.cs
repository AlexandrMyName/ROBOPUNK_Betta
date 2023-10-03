using Abstracts;
using UniRx;
using System;
using UnityEngine;

namespace Core
{

    public class EnemyAttackComponent : IEnemyAttackable
    {

        public ReactiveProperty<float> Health { get; private set; }

        public ReactiveProperty<float> HealthProtection { get; set; }

        public ReactiveProperty<bool> IsDeadFlag { get; set; }

        public ReactiveProperty<bool> IsCameAttackPosition { get;  set;}

        public ReactiveProperty<bool> IsRewardReadyFlag { get; set; }
        

        public float AttackDistance { get; set; }

        public float AttackFrequency { get; set; }

        public bool IsIgnoreDamage { get ; set; }

        public float Damage { get; set; }
        public RaycastHit CachedHitDamage { get; set; }
        public Vector3 CachedDirectionDamage { get; set; }


        public EnemyAttackComponent(float health, float damage, float attackDistance, float attackFrequency)
        {

            HealthProtection = new ReactiveProperty<float>(0);
            Health = new ReactiveProperty<float>(health);
            IsDeadFlag = new ReactiveProperty<bool>(false);
            IsCameAttackPosition = new ReactiveProperty<bool>(false);
            IsRewardReadyFlag = new ReactiveProperty<bool>(false);
            Damage = damage;
            AttackDistance = attackDistance;
            AttackFrequency = attackFrequency;
        }


        public void SetAttackableDamage(float attackForceDamage) => Damage = attackForceDamage;


        public void SetMaxHealth(float maxHealth, Action<ReactiveProperty<float>> onCompleted = null)
        {

            if (Health == null)
                Health = new ReactiveProperty<float>(maxHealth);
            else
                Health.Value = maxHealth;

            Health.SkipLatestValueOnSubscribe();
            onCompleted.Invoke(Health);
        }


        [Obsolete]
        public void TakeDamage(float amountHealth)
        {

            if (IsIgnoreDamage) return;

            else
            {
                HealthProtection.Value -= amountHealth;

                if (HealthProtection.Value < 0)
                {
                    Health.Value -= MathF.Abs(HealthProtection.Value);
                    HealthProtection.Value = 0;
                }
            }
        }


        public void TakeDamage(float amountHealth, RaycastHit hit, Vector3 direction)
        {

            if (IsIgnoreDamage) return;

            else
            {
                HealthProtection.Value -= amountHealth;

                if (HealthProtection.Value < 0)
                {
                    CachedDirectionDamage = direction;
                    CachedHitDamage = hit;
                    Health.Value -= MathF.Abs(HealthProtection.Value);
                    HealthProtection.Value = 0;
                }
            }
        }
    }
}