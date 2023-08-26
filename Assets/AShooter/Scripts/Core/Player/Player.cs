using System;
using abstracts;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using StateMachine = abstracts.StateMachine;


namespace Core
{
    
    public sealed class Player : StateMachine
    {
        
        [Inject(Id = "PlayerSystems")] private List<ISystem> _systems;
        
        protected override List<ISystem> GetSystems() =>  _systems ;
        
        
    }
}