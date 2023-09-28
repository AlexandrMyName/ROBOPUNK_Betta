using Zenject;
using UnityEngine;
using Core;
using DI.Spawn;
using UniRx;
using User;


namespace DI
{

    public class EnemyInstaller : MonoInstaller
    {

        [SerializeField] private bool _spawnOnStart;
        [SerializeField] private EnemyDataConfig _enemyDataConfig;
        [SerializeField] private EnemySpawnerDataConfig _enemySpawnerDataConfig;

        private EnemySpawnerController _spawner;

        public override void InstallBindings()
        {
            Container.Bind<EnemySpawnerController>().FromComponentInHierarchy().AsSingle();
        }


        public override void Start()
        {
            GameLoopManager.SetEnemyMaxHealth(100);
            GameLoopManager.SetEnemyDamageForce(100);
            GameLoopManager.SetAttackFrequency(1);

            
            if (_spawnOnStart)
            {
                _spawner = new EnemySpawnerController(_enemyDataConfig, _enemySpawnerDataConfig);
                _spawner.StartSpawnProcess();
            }
                

        }

    }

}