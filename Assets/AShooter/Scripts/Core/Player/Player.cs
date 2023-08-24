using abstracts;
using System.Collections.Generic;
using Zenject;

namespace Core
{
    public class Player : StateMachine
    {
        private List<ISystem> _systems;

        [Inject]
        private void Initialize(List<ISystem> systems) => _systems = systems;

        protected override List<ISystem> GetSystems() => _systems;
    }
}