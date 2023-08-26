using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Core;
using abstracts;
using DI.Spawn;
using Cinemachine;


namespace DI
{
    
    public class PlayerInstaller : MonoInstaller
    {
        
        [SerializeField] private bool _spawnOnAwake;
        [SerializeField] private Spawner _spawner;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [Space(10), SerializeField] private bool _useMoveSystem;
        [SerializeField] private bool _useShootSystem;


        public override void InstallBindings()
        {
            Container
                .Bind<List<ISystem>>()
                .WithId("PlayerSystems")
                .FromInstance(InitSystems())
                .AsCached();
        }
        
        
        private List<ISystem> InitSystems()
        {
            List<ISystem> systems = new List<ISystem>();
            
            InputSystem inputSystem = new InputSystem();
            Container.QueueForInject(inputSystem);
            
            PlayerMovementSystem moveSystem = new PlayerMovementSystem();
            Container.QueueForInject(moveSystem);
            
            PlayerShootSystem shootSystem = new PlayerShootSystem();
            Container.QueueForInject(shootSystem);
            
            systems.Add(inputSystem);
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
 
