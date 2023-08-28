using Abstracts;
using System.Collections.Generic;
using UniRx;
using Zenject;


namespace Core
{

    public class Enemy : StateMachine, IAttackable
    {
        
        [Inject(Id = "EnemySystems")] private List<ISystem> _systems;
        [Inject(Id = "EnemyHealth")] public ReactiveProperty<float> Health { get; }
      
        protected override List<ISystem> GetSystems() => _systems;

        public void TakeDamage(float amountHealth) => Health.Value -= amountHealth;


    }
}