using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;
using StateMachine = Abstracts.StateMachine;


namespace Core
{
    
    public sealed class Player : StateMachine, IPlayer, IDisposable
    {

        [Inject(Id = "PlayerComponents")] public IComponentsStore ComponentsStore { get; private set; }

        [Inject(Id = "PlayerSystems")] private List<ISystem> _systems;

        [field: SerializeField] public Transform WeaponContainer;
         

        protected override List<ISystem> GetSystems()
        {

            ComponentsStore.Movable.InitComponent(GetComponent<Rigidbody>());
            ComponentsStore.PlayerHP.IsAlive.Subscribe(value => {
                if(value == false) Dispose();
            }).AddTo(this);

            return _systems;
        }


        public void Dispose()
        {

            foreach(var system in _systems)
            {
               var disposableSystem = (IDisposable) system;

                if(disposableSystem != null)
                {
                    disposableSystem.Dispose();
                }
            }

            this.enabled = false;
        }

    }
}