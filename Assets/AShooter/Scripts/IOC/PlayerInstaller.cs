using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Core;
using Abstracts;
using AShooter.Scripts.Core.Player.Components;
using AShooter.Scripts.IOC;
using DI.Spawn;
using Cinemachine;
using Core.Components;
using TMPro;
using UniRx;
using UnityEngine.Serialization;
using User;
using User.Components;
using User.Components.Repository;

namespace DI
{
    
    public class PlayerInstaller : MonoInstaller
    {

        [FormerlySerializedAs("_loadPlayerStatsFromRepository")]
        [Header("Set True - to use player stats multiplier from repository \n" +
                "Set False - to use default stats ")]
        [SerializeField] private bool _loadMetaMultipliersFromRepository;
        [Space(20)]

        [SerializeField] private DashConfig _dashConfig;
        [SerializeField] private PlayerHPConfig _playerHPConfig;
        [SerializeField] private ExperienceConfig _experienceConfig;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Spawner _spawner;

        [Space(10), SerializeField] private bool _useMoveSystem;
        [SerializeField] private bool _useShootSystem;
        [SerializeField] private bool _useRotationSystem;

        [SerializeField] private float _maxPlayerHealth;
        [SerializeField] private float _maxPlayerProtection = 50f;
        [SerializeField] private float _protectionRegenerationTime = 7f;
        [SerializeField] private float _jumpHeight = 2.6f;
        [SerializeField] private float _jumpTime = 3f;
        [SerializeField] private float _speed;
        
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _spawnTransform;

        private SpawnPlayerFactory _spawnPlayerFactory;
        
        private Player _player;

        [Space(10)]
        [Header("Progress Configs:")]
        [SerializeField] private LevelRewardItemsConfigs _levelRewardItemsConfigs;
        [SerializeField] private float requiredExperienceForNextLevel = 10f;
        [SerializeField] private float _progressRate = 2.0f;


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

            PlayerMoveComponent movable = new PlayerMoveComponent(_jumpHeight,_jumpTime);
            PlayerAttackComponent attackable = new PlayerAttackComponent();
            PlayerDashComponent dash = new PlayerDashComponent(_dashConfig);
            PlayerHPComponent playerHP = new PlayerHPComponent(_playerHPConfig);
            ViewsComponent views = new ViewsComponent();
            PlayerGoldWalletComponent gold = new PlayerGoldWalletComponent();
            PlayerExperienceComponent exp = new PlayerExperienceComponent(_experienceConfig);
            WeaponStorage weapons = new WeaponStorage();
            PlayerLevelRewardComponent levelReward = new PlayerLevelRewardComponent(_levelRewardItemsConfigs);
            PlayerLevelProgressComponent levelProgress = new PlayerLevelProgressComponent(requiredExperienceForNextLevel, _progressRate);
            PlayerShieldComponent shield = new PlayerShieldComponent(_maxPlayerProtection, _protectionRegenerationTime);

            var repository = new Repository();
            IPlayerStats playerStatsSaved = repository.Load();

            IPlayerStats playerStatsRuntime = new PlayerStatsComponent(
                playerStatsSaved.Name,
                playerStatsSaved.Money,
                playerStatsSaved.Experience,
                0,
                gold.CurrentGold,
                exp.CurrentExperience,
                playerStatsSaved.MetaExperience,
                playerStatsSaved.BaseHealthMultiplier,
                playerStatsSaved.BaseDamageMultiplier,
                playerStatsSaved.BaseMoveSpeedMultiplier,
                playerStatsSaved.BaseShieldCapacityMultiplier,
                playerStatsSaved.BaseDashDistanceMultiplier,
                playerStatsSaved.BaseShootSpeedMultiplier);

            Container.QueueForInject(movable);
            Container.QueueForInject(attackable);
            Container.QueueForInject(views);
            Container.QueueForInject(weapons);
            Container.QueueForInject(playerStatsRuntime);

            ComponentsStore components = new ComponentsStore(attackable, movable, dash, playerHP, 
                views, gold, exp, levelReward, levelProgress, weapons, shield, playerStatsRuntime);

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

            PlayerMenuSystem playerStoreSystem = new PlayerMenuSystem();
            Container.QueueForInject(playerStoreSystem);

            PlayerExperienceSystem playerExperienceSystem = new PlayerExperienceSystem();
            Container.QueueForInject(playerExperienceSystem);

            PlayerGoldWalletSystem playerGoldWalletSystem = new PlayerGoldWalletSystem();
            Container.QueueForInject(playerGoldWalletSystem);

            PlayerShieldSystem playerShieldSystem = new PlayerShieldSystem();
            Container.QueueForInject(playerShieldSystem);

            MP3PlayerSystem mp3PlayerSystem = new MP3PlayerSystem();
            Container.QueueForInject(mp3PlayerSystem);

            PickUpItemSystem pickUpItemSystem = new PickUpItemSystem();
            Container.QueueForInject(pickUpItemSystem);

            PlayeLevelRewardSystem playeRewardSystem = new PlayeLevelRewardSystem();
            Container.QueueForInject(playeRewardSystem);

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
            systems.Add(playerShieldSystem);
            systems.Add(mp3PlayerSystem);
            systems.Add(pickUpItemSystem);
            systems.Add(playeRewardSystem);

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
 
