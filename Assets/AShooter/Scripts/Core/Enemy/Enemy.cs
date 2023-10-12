using Abstracts;
using Core.DTO;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core
{


    public class Enemy : StateMachine, IEnemy
    {
        
        public EnemyType EnemyType { get; set; }

        public EnemyState EnemyState { get; set; }

        public IEnemyComponentsStore ComponentsStore { get; private set; }

        private List<ISystem> _systems;


        public void SetComponents(IEnemyComponentsStore components)
        {
            ComponentsStore = components;
            
        }


        public void SetSystems(List<ISystem> systems) => _systems = systems;


        protected override List<ISystem> GetSystems() =>  _systems;


    }
}