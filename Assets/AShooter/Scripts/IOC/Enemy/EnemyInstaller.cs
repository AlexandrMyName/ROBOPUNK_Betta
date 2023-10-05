using Zenject;
using UnityEngine;
using DI.Spawn;
using User;
using Abstracts;
using Core;


namespace DI
{

    public class EnemyInstaller : MonoInstaller
    {

        [Inject] private DiContainer _container;
        [Inject(Id = "PlayerComponents")] private IComponentsStore _componentsPlayer;

        [SerializeField] private bool _spawnOnStart;
        [SerializeField] private EnemySpawnerDataConfig _enemySpawnerDataConfig;
        [SerializeField] private GameObject _enemyViews_Prefab;
        private EnemySpawnerController _spawner;


        public override void InstallBindings()
        {
            //Container.Bind<EnemySpawnerController>()
            //    .FromInstance(_spawner)
            //    .AsCached();
        }


        public override void Start()
        {
            if (_spawnOnStart)
            {
                _spawner = new EnemySpawnerController(
                    _enemySpawnerDataConfig, 
                    _container, 
                    _componentsPlayer.Movable.Rigidbody.transform,
                    _componentsPlayer.GoldWallet,
                    _componentsPlayer.ExperienceHandle);
                _spawner.StartSpawnProcess(_enemyViews_Prefab);
                
            }
        }
        private void OnDestroy()
        {
            _spawner.StopSpawnProcess();
        }

    }
}