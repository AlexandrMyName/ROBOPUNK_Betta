using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using UniRx;
using System;
using Zenject;

namespace Core
{


    public class EnemyAttackComponent : IEnemyAttackable
    {
        public ReactiveProperty<float> Health { get; set; }

        public ReactiveProperty<bool> IsDeadFlag { get; set; }

        public ReactiveProperty<bool> IsCameAttackPosition { get;  set;}

        public float Damage { get; set; }

        public ReactiveProperty<float> RangedAttackRange { get; private set; }
        public ReactiveProperty<float> MeleeAttackRange { get; private set; }


        public void InitComponent(float rangedAttackRange, float meleeAttackRange)
        {
            RangedAttackRange = new ReactiveProperty<float>(rangedAttackRange);
            MeleeAttackRange = new ReactiveProperty<float>(meleeAttackRange);
            IsDeadFlag = new ReactiveProperty<bool>(false);
            IsCameAttackPosition = new ReactiveProperty<bool>(false);
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