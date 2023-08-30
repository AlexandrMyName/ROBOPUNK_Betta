using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Core;
using Abstracts;
using DI.Spawn;
using Cinemachine;
using User;


namespace DI
{
    
    public class PlayerInstaller : MonoInstaller
    {
        
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private Spawner _spawner;
        
        [Space(10), SerializeField] private bool _useMoveSystem;
        [SerializeField] private bool _spawnOnAwake;
        [SerializeField] private bool _useShootSystem;
        
        
        public override void InstallBindings()
        {
            Container
                .Bind<List<ISystem>>()
                .WithId("PlayerSystems")
                .FromInstance(InitSystems())
                .AsCached();
            
            Container.Bind<IWeapon>().FromInstance(
                new Weapon(
                    _weaponConfig.WeaponPrefab, 
                    _weaponConfig.LayerMask, 
                    _weaponConfig.EffectPrefab, 
                    _weaponConfig.Damage, 
                    _weaponConfig.EffectDestroyDelay)
                )
                .AsCached();
        }
        
        
        private List<ISystem> InitSystems()
        {
            List<ISystem> systems = new List<ISystem>();

            PlayerMovementSystem moveSystem = new PlayerMovementSystem();
            Container.QueueForInject(moveSystem);
            
            PlayerShootSystem shootSystem = new PlayerShootSystem();
            Container.QueueForInject(shootSystem);
            
            systems.Add(moveSystem);
            systems.Add(shootSystem);

            return systems;
        }

        
        private void Awake()
        {
            
            if (_spawnOnAwake)
            {
               GameObject player = _spawner.Spawn();
                _camera.Follow = player.transform;
                _camera.LookAt = player.transform;
            }
        }
        
        
    }
}
 
