using Zenject;
using UnityEngine;
using Core;
using DI.Spawn;
using UniRx;

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
            GameLoopManager.SetAttackFrequency(_attackFrequency);

            SetRangedAttackRange(_rangedAttackRange);
            SetMeleeAttackRange(_meleeAttackRange);

            if (_spawnOnStart)
                _spawner.StartSpawnProcess();
        }


        private void SetRangedAttackRange(float rangedAttackRange)
        {
            ReactiveProperty<float> ranged = new ReactiveProperty<float>(rangedAttackRange);

            Container
                .BindInstance(ranged)
                .WithId("EnemyRangedAttackRange")
                .AsCached();
        }


        private void SetMeleeAttackRange(float meleeAttackRange)
        {
            ReactiveProperty<float> ranged = new ReactiveProperty<float>(meleeAttackRange);

            Container
                .BindInstance(ranged)
                .WithId("EnemyMeleeAttackRange")
                .AsCached();
        }


    }

}