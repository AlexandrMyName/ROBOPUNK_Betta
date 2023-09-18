using Abstracts;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using StateMachine = Abstracts.StateMachine;


namespace Core
{
    
    public sealed class Player : StateMachine, IPlayer 
    {

        [Inject(Id = "PlayerComponents")] public IComponentsStore ComponentsStore { get; private set; }

        
        [Inject(Id = "PlayerSystems")] private List<ISystem> _systems;

         
        [field: SerializeField] public Transform WeaponContainer;
         

        protected override List<ISystem> GetSystems()
        {
            ComponentsStore.Movable.InitComponent(GetComponent<Rigidbody>());


           return _systems;
        }

    }
}