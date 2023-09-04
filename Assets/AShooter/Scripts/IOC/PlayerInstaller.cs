using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Core;
using Abstracts;
using DI.Spawn;
using Cinemachine;
using User;
using UniRx;
using static UnityEditor.Experimental.GraphView.GraphView;


namespace DI
{
    
    public class PlayerInstaller : MonoInstaller
    {
        
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private Spawner _spawner;
        
        [Space(10), SerializeField] private bool _useMoveSystem;
        [SerializeField] private bool _useShootSystem;
        [SerializeField] private float _maxPlayerHealth;
        [SerializeField] private float _speed;
        private GameObject _player;
        public override void InstallBindings()
        {
            SetHealth(_maxPlayerHealth);
            SetSpeed(_speed);

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

            PlayerHealthSystem healthSystem = new PlayerHealthSystem();
            Container.QueueForInject(healthSystem);
            
            systems.Add(moveSystem);
            systems.Add(shootSystem);
            systems.Add(healthSystem);

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

        private Transform GetPlayerTransform()
        {
            return _player.transform;
        }

        private void Awake()
        {   
             
                _player = _spawner.Spawn();
                _camera.Follow = _player.transform;
                _camera.LookAt = _player.transform;
             
                Container.Bind<Transform>().WithId("PlayerTransform").FromInstance(GetPlayerTransform()).AsCached();
        }
        
        
    }
}
 
