using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core
{
    public class Enemy : StateMachine, IAttackable
    {

        [Inject(Id = "PlayerTransform")] public Transform playerTransform;

        [SerializeField] private SphereCollider _enemyRadiusAttack;
        private ReactiveProperty<float> _health;
        private float _attackForce;

        [HideInInspector] public ReactiveProperty<float> Health { get => _health; set => _health = value; }
        [field: SerializeField] public ReactiveProperty<bool> IsDeadFlag { get; set; }
        [SerializeField] public ReactiveProperty<bool> IsReadyToStrike { get; set; }
        [SerializeField] public ReactiveProperty<bool> IsReadyToShoot { get; set; }
        public SphereCollider EnemyRadiusAttack => _enemyRadiusAttack;
        public float Damage => _attackForce;


        public void SetMaxHealth(float maxHealth, Action<ReactiveProperty<float>> onCompleted = null)
        {
            if (_health == null)
                _health = new ReactiveProperty<float>(maxHealth);
            else
                _health.Value = maxHealth;

            _health.SkipLatestValueOnSubscribe();
            onCompleted.Invoke(Health);
        }


        public void SetAttackableDamage(float attackForceDamage) => _attackForce = attackForceDamage;


        public void TakeDamage(float amountHealth) => Health.Value -= amountHealth;


        protected override List<ISystem> GetSystems()
        {
            var systems = new List<ISystem>();
            systems.Add(new EnemyMovementSystem());
            systems.Add(new EnemyDamageSystem());
            systems.Add(new EnemyMeleeAttackSystem());
            return systems;
        }


    }

}