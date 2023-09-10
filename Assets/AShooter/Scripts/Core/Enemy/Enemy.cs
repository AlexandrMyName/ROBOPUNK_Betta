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
        [Inject(Id = "EnemyRangedAttackRange")] private ReactiveProperty<float> _rangedAttackRange;
        [Inject(Id = "EnemyMeleeAttackRange")] private ReactiveProperty<float> _meleeAttackRange;
        [Inject(Id = "PlayerTransform")] private Transform _playerTransform;


        [SerializeField] private SphereCollider _enemyRadiusAttack;
        private ReactiveProperty<float> _health;
        private List<ISystem> _systems;
        private float _attackForce;


        [HideInInspector] public ReactiveProperty<float> Health { get => _health; set => _health = value; }
        [field: SerializeField] public ReactiveProperty<bool> IsDeadFlag { get; set; }
        public Transform PlayerTransform { get { return _playerTransform; } }
        public SphereCollider EnemyRadiusAttack => _enemyRadiusAttack;
        public ReactiveProperty<float> RangedAttackRange { get { return _rangedAttackRange; } }
        public ReactiveProperty<float> MeleeAttackRange { get { return _meleeAttackRange; } }
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


        protected override List<ISystem> GetSystems() => _systems;


        public void SetSystems(List<ISystem> systems)
        {
            _systems = systems;
            Debug.Log("6");
            Debug.Log($"_systems == null - {_systems == null}");
        }


    }

}