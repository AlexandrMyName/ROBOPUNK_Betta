using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Core;
using Abstracts;
using DI.Spawn;
using UniRx;


namespace DI
{
    
    public class EnemyInstaller : MonoInstaller
    {
        
        [SerializeField] private Spawner _spawner;
        
        [SerializeField] private bool _spawnOnAwake;
        [SerializeField] private float _maxHealth;


        public override void InstallBindings()
        {
            SetHealth(_maxHealth);
            
            Container.Bind<List<ISystem>>()
                .WithId("EnemySystems")
                .FromInstance(InitSystem())
                .AsCached();
        }
            


        private List<ISystem> InitSystem()
        {
            List<ISystem> systems = new List<ISystem>();

            var enemyMovable = new EnemyMovementSystem();
            var enemyDamage = new EnemyDamageSystem();

            Container.QueueForInject(enemyMovable);
            Container.QueueForInject(enemyDamage);
            
            systems.Add(enemyMovable);
            systems.Add(enemyDamage);
            
            return systems;
        }
        
        
        private void SetHealth(float initializedMaxHealth)
        {
            ReactiveProperty<float> health = new ReactiveProperty<float>(initializedMaxHealth);
            
            Container
                .BindInstance(health)
                .WithId("EnemyHealth")
                .AsCached();
        }

        
        private void Awake()
        {
            if (_spawnOnAwake)
                _spawner.Spawn();
        }
        
        
    }
}

