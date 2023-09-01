using Abstracts;
using System.Collections.Generic;
using UniRx;
using Zenject;
using StateMachine = Abstracts.StateMachine;


namespace Core
{
    
    public sealed class Player : StateMachine, IAttackable
    {
        
        [Inject(Id = "PlayerSystems")] private List<ISystem> _systems;
        [Inject(Id = "PlayerHealth")] public ReactiveProperty<float> Health { get; }


        protected override List<ISystem> GetSystems() =>  _systems;


        public void TakeDamage(float amountDamage)
        {
            Health.Value -= amountDamage;
        }


    }
}