using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Core;
using Abstracts;
using AShooter.Scripts.IOC;
using DI.Spawn;
using Cinemachine;
using Core.Components;
using UniRx;
using User;
using User.Components;

namespace DI
{
    
    public class PlayerInstaller : MonoInstaller
    {

        [SerializeField] private StoreItemsDataConfig _storeItemsDataConfig;
        [SerializeField] private DashConfig _dashConfig;
        [SerializeField] private PlayerHPConfig _playerHPConfig;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Spawner _spawner;

        [Space(10), SerializeField] private bool _useMoveSystem;
        [SerializeField] private bool _useShootSystem;
        [SerializeField] private bool _useRotationSystem;

        [SerializeField] private float _maxPlayerHealth;
        [SerializeField] private float _speed;
        
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _spawnTransform;
        
        private SpawnPlayerFactory _spawnPlayerFactory;
        
        private Player _player;

        [Space,SerializeField, Header("Test (can be bull)")]
        private World _world;


        public override void InstallBindings()
        {
            SetHealth(_maxPlayerHealth);
            SetSpeed(_speed);

            Container
                .Bind<IComponentsStore>()
                .WithId("PlayerComponents")
                .FromInstance(InitComponents())
                .AsCached();

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
        

        private IComponentsStore InitComponents()
        {

            PlayerMoveComponent movable = new PlayerMoveComponent();
            PlayerAttackComponent attackable = new PlayerAttackComponent();
            PlayerDashComponent dash = new PlayerDashComponent(_dashConfig);
            PlayerHPComponent playerHP = new PlayerHPComponent(_playerHPConfig);
            ViewsComponent views = new ViewsComponent();
            PlayerGoldWalletComponent gold = new PlayerGoldWalletComponent();
            PlayerExperienceComponent exp = new PlayerExperienceComponent();
            WeaponStorage weapons = new WeaponStorage();
            PlayerStoreEnhancementComponent store = new PlayerStoreEnhancementComponent(_storeItemsDataConfig);

            Container.QueueForInject(movable);
            Container.QueueForInject(attackable);
            Container.QueueForInject(views);
            Container.QueueForInject(weapons);

            ComponentsStore components = new ComponentsStore(attackable, movable, dash, playerHP, views, gold, exp, store, weapons);

            return components;
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

            ExplosionAbilitySystem explosionAbilitySystem = new ExplosionAbilitySystem();
            Container.QueueForInject(explosionAbilitySystem);

            PlayerImprovementSystem improvementSystem = new PlayerImprovementSystem();
            Container.QueueForInject(improvementSystem);

            PlayerInventorySystem inventorySystem = new PlayerInventorySystem();
            Container.QueueForInject(inventorySystem);
            
            PlayerRotationSystem rotationSystem = new PlayerRotationSystem();
            Container.QueueForInject(rotationSystem);
            
            PlayerMeleeAttackSystem meleeAttackSystem = new PlayerMeleeAttackSystem();
            Container.QueueForInject(meleeAttackSystem);

            PlayerDashSystem dashSystem = new PlayerDashSystem();
            Container.QueueForInject(dashSystem);

            PlayerStoreSystem playerStoreSystem = new PlayerStoreSystem();
            Container.QueueForInject(playerStoreSystem);

            PlayerExperienceSystem playerExperienceSystem = new PlayerExperienceSystem();
            Container.QueueForInject(playerExperienceSystem);

            PlayerGoldWalletSystem playerGoldWalletSystem = new PlayerGoldWalletSystem();
            Container.QueueForInject(playerGoldWalletSystem);

            systems.Add(moveSystem);
            systems.Add(shootSystem);
            systems.Add(healthSystem);
            systems.Add(weaponSystem);
            systems.Add(improvementSystem);
            systems.Add(explosionAbilitySystem);
            systems.Add(inventorySystem);
            systems.Add(meleeAttackSystem);
            systems.Add(dashSystem);
            systems.Add(rotationSystem);
            systems.Add(playerStoreSystem);
            systems.Add(playerExperienceSystem);
            systems.Add(playerGoldWalletSystem);

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

        
        public void Awake()
        {

            _spawnPlayerFactory = Container.Resolve<SpawnPlayerFactory>();
            _player = _spawnPlayerFactory.Create();

            _camera.Follow = _player.transform;
            _camera.LookAt = _player.transform;
            
            Container.Bind<Transform>()
                .WithId("PlayerTransform")
                .FromInstance(_player.transform)
                .AsCached();

            Container.Bind<Player>()
                .FromInstance(_player)
                .AsCached();

            if (_world) _world.Init(_player.transform);
        }
        
        
    }
}
 
