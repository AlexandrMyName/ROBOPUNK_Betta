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
        [SerializeField] private Collider _enemyRadiusAttack;

        private float _attackForce;
        private ReactiveProperty<float> _health;
        [HideInInspector] public ReactiveProperty<float> Health { get => _health; set => _health = value; }
        [field: SerializeField] public ReactiveProperty<bool> IsDeadFlag { get; set; }
        public float Damage => _attackForce;

        [Inject(Id = "PlayerTransform")] public Transform playerTransform;
         
        public Collider EnemyRadiusAttack => _enemyRadiusAttack;
        public void TakeDamage(float amountHealth) => Health.Value -= amountHealth;

        public void SetMaxHealth(float maxHealth, Action<ReactiveProperty<float>> onCompleted = null)
        {
            if(_health == null)
                _health = new ReactiveProperty<float>(maxHealth);
            else
            {
                _health.Value = maxHealth;
            }

            _health.SkipLatestValueOnSubscribe();
            onCompleted.Invoke(Health);
        }
        public void SetAttackableDamage(float attackForceDamage) => _attackForce = attackForceDamage;
        
        protected override List<ISystem> GetSystems()
        {
            var systems = new List<ISystem>();
            systems.Add(new EnemyMovementSystem());
            systems.Add(new EnemyDamageSystem());
            systems.Add(new EnemyAttackSystem());
            return systems;
        }
        
    }
}