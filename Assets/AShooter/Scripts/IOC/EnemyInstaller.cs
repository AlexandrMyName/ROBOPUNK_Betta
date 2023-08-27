using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Core;
using abstracts;
using DI.Spawn;

namespace DI
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private bool _spawnOnAwake;
        [SerializeField] private Spawner _spawner;

        public override void InstallBindings()
        => Container.Bind<List<ISystem>>().WithId("EnemySystems").FromInstance(InitSystem()).AsCached();

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

        private void Awake()
        {
            if (_spawnOnAwake)
                _spawner.Spawn();
        }
    }
}

