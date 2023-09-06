using Zenject;
using UnityEngine;
using Core;
using DI.Spawn;


namespace DI
{
    
    public class EnemyInstaller : MonoInstaller
    {
        
        [SerializeField] private EnemySpawner _spawner;        
        [SerializeField] private bool _spawnOnStart;
        [SerializeField] private float _maxHealthOnNewGame;
        [SerializeField] private float _maxAttackDamageOnNewGame;

        
        public override void InstallBindings()
        {
            Container.Bind<EnemySpawner>().FromComponentInHierarchy().AsSingle();
        }
        

        public override void Start()
        {
            GameLoopManager.SetEnemyMaxHealth(_maxHealthOnNewGame);
            GameLoopManager.SetEnemyDamageForce(_maxAttackDamageOnNewGame);
            if (_spawnOnStart)
                _spawner.StartSpawnProcess();
        }
        
    }

}