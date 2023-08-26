using abstracts;
using System.Collections.Generic;
using Zenject;


namespace Core
{
    
    public class Player : StateMachine
    {
        
        [Inject(Id = "PlayerSystems")] private List<ISystem> _systems;

        
        protected override List<ISystem> GetSystems() =>  _systems ;
        
        
    }
}