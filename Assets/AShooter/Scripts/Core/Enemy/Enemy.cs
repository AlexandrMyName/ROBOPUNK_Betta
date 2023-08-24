using abstracts;
using System.Collections.Generic;
using Zenject;

namespace Core
{

    public class Enemy : StateMachine
    {
        [Inject(Id = "EnemySystems")] private List<ISystem> _systems;
        protected override List<ISystem> GetSystems() => _systems;
    }
}