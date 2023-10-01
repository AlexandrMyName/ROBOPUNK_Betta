using Abstracts;
using UniRx;
using System;


namespace Core
{

    public class EnemyAttackComponent : IEnemyAttackable
    {

        public ReactiveProperty<float> Health { get; private set; }

        public ReactiveProperty<bool> IsDeadFlag { get; set; }

        public ReactiveProperty<bool> IsCameAttackPosition { get;  set;}

        public ReactiveProperty<float> RangedAttackRange { get; private set; }

        public float AttackDistance { get; set; }

        public float AttackFrequency { get; set; }

        public ReactiveProperty<bool> IsRewardReadyFlag { get; set; }

        public bool IsIgnoreDamage { get ; set; }

        public float Damage { get; set; }


        public EnemyAttackComponent(float health, float damage, float attackDistance, float attackFrequency)
        {
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


        public void TakeDamage(float amountHealth) => Health.Value -= amountHealth;


    }
}