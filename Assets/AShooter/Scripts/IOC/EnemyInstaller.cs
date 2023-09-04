using Zenject;
using UnityEngine;
using Core;
using DI.Spawn;


namespace DI
{
    
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private EnemySpawner _spawner;        
        [SerializeField] private bool _spawnOnAwake;
        [SerializeField] private float _maxHealthOnNewGame;

        public override void InstallBindings()
        {
            Container.Bind<EnemySpawner>().FromComponentInHierarchy().AsSingle();
        }

        

        private void Awake()
        {
            GameLoopManager.SetEnemyMaxHealth(_maxHealthOnNewGame);
            if (_spawnOnAwake)
                _spawner.StartSpawnProcess();
        }
    }
}

