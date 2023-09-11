using System;
using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Core;
using Abstracts;
using AShooter.Scripts.IOC;
using DI.Spawn;
using Cinemachine;
using User;
using UniRx;


namespace DI
{
    
    public class PlayerInstaller : MonoInstaller
    {
        
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Spawner _spawner;
        
        [Space(10), SerializeField] private bool _useMoveSystem;
        [SerializeField] private bool _useShootSystem;
        [SerializeField] private float _maxPlayerHealth;
        [SerializeField] private float _speed;
        
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _spawnTransform;
        
        private SpawnPlayerFactory _spawnPlayerFactory;
        
        private GameObject _playerObject;
        private Player _player;
        
        
        public override void InstallBindings()
        {
            SetHealth(_maxPlayerHealth);
            SetSpeed(_speed);

            Container
                .Bind<List<ISystem>>()
                .WithId("PlayerSystems")
                .FromInstance(InitSystems())
                .AsCached();

            Container.BindFactory<Player, SpawnPlayerFactory>()
                .FromComponentInNewPrefab(_prefab)
                .WithGameObjectName("Player");

            Container
                .Bind<CinemachineVirtualCamera>()
                .FromInstance(_camera)
                .AsCached();
        }
        
        
        private List<ISystem> InitSystems()
        {
            List<ISystem> systems = new List<ISystem>();

            PlayerMovementSystem moveSystem = new PlayerMovementSystem();
            Container.QueueForInject(moveSystem);

            PlayerWeaponSystem weaponSystem = new PlayerWeaponSystem();
            Container.QueueForInject(weaponSystem);
            
            PlayerShootSystem shootSystem = new PlayerShootSystem();
            Container.QueueForInject(shootSystem);

            PlayerHealthSystem healthSystem = new PlayerHealthSystem();
            Container.QueueForInject(healthSystem);

            PlayerImprovementSystem improvementSystem = new PlayerImprovementSystem();
            Container.QueueForInject(improvementSystem);
            systems.Add(moveSystem);
            systems.Add(shootSystem);
            systems.Add(healthSystem);
            systems.Add(weaponSystem);
            systems.Add(improvementSystem);

            return systems;
        }


        private void SetHealth(float initializedMaxHealth)
        {
            ReactiveProperty<float> health = new ReactiveProperty<float>(initializedMaxHealth);

            Container
                .BindInstance(health)
                .WithId("PlayerHealth")
                .AsCached();
        }

        
        private void SetSpeed(float initSpeed)
        {
            ReactiveProperty<float> speed = new ReactiveProperty<float>(initSpeed);

            Container
                .BindInstance(speed)
                .WithId("PlayerSpeed")
                .AsCached();
        }

        
        public override void Start()
        {
            _spawnPlayerFactory = Container.Resolve<SpawnPlayerFactory>();
            _player = _spawnPlayerFactory.Create();
            
            _camera.Follow = _player.transform;
            _camera.LookAt = _player.transform;
            
            Container.Bind<Transform>().WithId("PlayerTransform").FromInstance(_player.transform).AsCached();
            Container.Bind<Player>().FromInstance(_player).AsCached();
        }
        
        
    }
}
 
