using Abstracts;
using Core.DTO;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Core
{


    public class Enemy : StateMachine, IEnemy
    {

        [field: SerializeField] public SphereCollider EnemyRadiusAttack { get; private set; }

        public EnemyType EnemyType { get; set; }

        public EnemyState EnemyState { get; set; }

        public IEnemyComponentsStore ComponentsStore { get; private set; }

        [Inject(Id = "PlayerTransform")] public Transform PlayerTransform { get; private set; }

        private List<ISystem> _systems;

        public void SetComponents(IEnemyComponentsStore components,  float rangeRadius)
        {
            ComponentsStore = components;
            ComponentsStore.Attackable.InitComponent(rangeRadius);
        }


        public void SetSystems(List<ISystem> systems) => _systems = systems;


        protected override List<ISystem> GetSystems() =>  _systems;

    }

}