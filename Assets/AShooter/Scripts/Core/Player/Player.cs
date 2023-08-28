using Abstracts;
using System.Collections.Generic;
using Zenject;
using StateMachine = Abstracts.StateMachine;


namespace Core
{
    
    public sealed class Player : StateMachine
    {
        
        [Inject(Id = "PlayerSystems")] private List<ISystem> _systems;
        
        protected override List<ISystem> GetSystems() =>  _systems ;
        
        
    }
}