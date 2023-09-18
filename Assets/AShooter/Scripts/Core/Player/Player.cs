using Abstracts;
using System;
using System.Collections.Generic;
using Core.Components;
using UniRx;
using UnityEngine;
using Zenject;
using StateMachine = Abstracts.StateMachine;


namespace Core
{
    
    public sealed class Player : StateMachine, IAttackable, IPlayer 
    {
        public ReactiveProperty<bool> IsDeadFlag { get; set; }

        [field: SerializeField] public Rigidbody _rigidbody { get; private set; }

        [Inject(Id = "PlayerSystems")] private List<ISystem> _systems;
        [Inject(Id = "PlayerHealth")] public ReactiveProperty<float> Health { get; }

        public MoveComponent MoveComponent { get; private set; }


        [field: SerializeField] public Transform WeaponContainer;


        private void Awake()
        {
            MoveComponent = new MoveComponent(_rigidbody);
        }

         
        protected override List<ISystem> GetSystems() =>  _systems;


        public void TakeDamage(float amountDamage) => Health.Value -= amountDamage;
        

        public void SetMaxHealth(float maxHealth, Action<ReactiveProperty<float>> onCompleted = null) { }

        public void SetAttackableDamage(float attackForceDamage) { }

        
    }
}