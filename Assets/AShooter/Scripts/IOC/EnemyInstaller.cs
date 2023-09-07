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
        [SerializeField] private float _meleeAttackRange;
        [SerializeField] private float _rangedAttackRange;
        [SerializeField] private float _attackFrequency;


        public override void InstallBindings()
        {
            Container.Bind<EnemySpawner>().FromComponentInHierarchy().AsSingle();
        }


        public override void Start()
        {
            GameLoopManager.SetEnemyMaxHealth(_maxHealthOnNewGame);
            GameLoopManager.SetEnemyDamageForce(_maxAttackDamageOnNewGame);
            GameLoopManager.SetMeleeAttackRange(_meleeAttackRange);
            GameLoopManager.SetRangedAttackRange(_rangedAttackRange);
            GameLoopManager.SetAttackFrequency(_attackFrequency);

            if (_spawnOnStart)
                _spawner.StartSpawnProcess();
        }


    }

}