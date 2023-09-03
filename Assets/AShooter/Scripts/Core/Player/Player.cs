using Abstracts;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;
using StateMachine = Abstracts.StateMachine;


namespace Core
{
    
    public sealed class Player : StateMachine, IAttackable , IMovable
    {
        [SerializeField] private Rigidbody _rigidbody;
        
        [Inject(Id = "PlayerSystems")] private List<ISystem> _systems;
        [Inject(Id = "PlayerHealth")] public ReactiveProperty<float> Health { get; }

        [Inject(Id = "PlayerSpeed")] public ReactiveProperty<float> Speed { get; }

        protected override List<ISystem> GetSystems() =>  _systems;


        public void TakeDamage(float amountDamage)
        {
            Health.Value -= amountDamage;
        }
        
        public void Move(Vector3 direction)
        {
            _rigidbody.velocity = direction * Speed.Value;
        }
        
        
    }
}