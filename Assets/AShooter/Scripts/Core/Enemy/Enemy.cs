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
        private ReactiveProperty<float> _health;
        [HideInInspector] public ReactiveProperty<float> Health { get => _health; set => _health = value; }
        [field: SerializeField] public ReactiveProperty<bool> IsDeadFlag { get; set; }

        [Inject(Id = "PlayerTransform")] public Transform playerTransform;


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
         
        protected override List<ISystem> GetSystems()
        {
            var systems = new List<ISystem>();
            systems.Add(new EnemyMovementSystem());
            systems.Add(new EnemyDamageSystem());
            return systems;
        }
        
    }
}