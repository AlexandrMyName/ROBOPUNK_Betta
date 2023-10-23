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

        private IPlayerStats _playerStats;
         


        [field: SerializeField] public Transform Headset;
         

        protected override List<ISystem> GetSystems()
        {

            ComponentsStore.Movable.InitComponent(GetComponent<Rigidbody>());
            ComponentsStore.PlayerHP.IsAlive.Subscribe(value => {
                if(value == false) Dispose();
            }).AddTo(this);

            _playerStats = ComponentsStore.PlayerStats;

            RecalculateBasicParametersBasedOnStatsMultipliers();
            
            return _systems;
        }

        
        private void RecalculateBasicParametersBasedOnStatsMultipliers()
        {
            ComponentsStore.Movable.Speed.Value *= _playerStats.BaseMoveSpeedMultiplier;
            ComponentsStore.Attackable.Health.Value *= _playerStats.BaseHealthMultiplier;
            ComponentsStore.Shield.MaxProtection *= _playerStats.BaseShieldCapacityMultiplier;
            ComponentsStore.Dash.DashForce *= _playerStats.BaseDashDistanceMultiplier;
            ComponentsStore.WeaponStorage.WeaponState.BasicDamageMultiplier = _playerStats.BaseDamageMultiplier;
            ComponentsStore.WeaponStorage.WeaponState.BasicShootSpeedMultiplier = _playerStats.BaseShootSpeedMultiplier;
            ComponentsStore.WeaponStorage.UpgradeWeaponsStatesAccordingPlayerBaseStats(_playerStats.BaseDamageMultiplier, _playerStats.BaseShootSpeedMultiplier);
        }


        public void Dispose()
        {
            foreach(var system in _systems)
            {
                IDisposable disposableSystem = (IDisposable) system;
                
                if(disposableSystem != null)
                {
                    disposableSystem.Dispose();
                }
            }
            this.enabled = false;
        }

        
        private void OnDestroy() => Dispose();
        
        
    }
}