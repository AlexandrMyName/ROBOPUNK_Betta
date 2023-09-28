using System;
using UnityEngine;
using UniRx;
using Core;
using Zenject;
using System.Collections.Generic;
using Abstracts;
using Core.DTO;
using UnityEngine.AI;
using UnityEngine.Pool;
using User;


namespace DI.Spawn
{

    public class EnemySpawner
    { 

        [Inject] private DiContainer _container;
        [Inject(Id = "PlayerComponents")] private IComponentsStore _componentsPlayer;

        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _spawnTransform;
        [SerializeField] private float _respawnDelay;
        
        [SerializeField] private int _numberMeleeEnemy;
        [SerializeField] private int _numberDistantEnemy;
        [SerializeField] private float _spawnRadius = 2f;
        [SerializeField, Range(1.5f, 7f)] private float _rangeRadiusRange;

        [SerializeField] private GameObject _spiderPrefab;
        [SerializeField, Range(0, 1)] private float _spiderProbableInstance;
        [SerializeField, Range(1.5f, 2.5f)] private float _spiderRadius;
        [SerializeField, Range(1, 100)] int _goldDropRate;

        private GameObjectPool _enemyPool;
        private float _numberMeleeEnemy_cnt;
        private float _numberDistantEnemy_cnt;
        private IDisposable _spawnDisposable;
        private Transform _playerTransform;
        private int activeEnemyCount = 0;
        private int _poolSize;

        private ObjectPool<GameObject> _pool;


        internal void StartSpawnProcess()
        {
            _poolSize = _numberMeleeEnemy + _numberDistantEnemy;
            _numberMeleeEnemy_cnt = _numberMeleeEnemy;
            _numberDistantEnemy_cnt = _numberDistantEnemy;

            _pool = new ObjectPool<GameObject>(
                createFunc: () => CreateEnemy(), 
                actionOnGet: (obj) => OnTakeFromPool(obj), 
                actionOnRelease: (obj) => OnReturnedToPool(obj), 
                actionOnDestroy: (obj) => OnDestroyPoolObject(obj), 
                collectionCheck: false, 
                defaultCapacity: _poolSize/2, 
                maxSize: _poolSize);

            _spawnDisposable = (IDisposable)Observable
                .Interval(TimeSpan.FromSeconds(_respawnDelay))
                .Where(_ => activeEnemyCount < 10)
                .Select(_ => _pool.Get());


            //_enemyPool = new GameObjectPool(() => CreateEnemy(), (_poolSize));


            //_spawnDisposable = Observable
            //    .Interval(TimeSpan.FromSeconds(_respawnDelay))
            //    .TakeUntilDestroy()
            //    .Subscribe(_ => TrySpawnEnemy());
        }



        private GameObject CreateEnemy()
        {
            GameObject enemyInstance = SpawnEnemy();

            SetTypeEnemy(enemyInstance);
            SetSystems(enemyInstance);
            SetModelEnemy(enemyInstance);
            GetPlayerTransform(enemyInstance);

            return enemyInstance;
        }



        private void OnDestroyPoolObject(GameObject obj)
        {
            throw new NotImplementedException();
        }


        private void OnReturnedToPool(GameObject obj)
        {
            throw new NotImplementedException();
        }


        private void OnTakeFromPool(GameObject obj)
        {
            throw new NotImplementedException();
        }


        internal void StopSpawning()
        {
            _spawnDisposable.Dispose();
        }


        internal void ReturnEnemyToPool(GameObject enemyInstance)
        {
            enemyInstance.SetActive(false);

            _enemyPool.Return(enemyInstance);
            activeEnemyCount--;
        }



        private void SetModelEnemy(GameObject enemyInstance)
        {
            var rend = enemyInstance.GetComponent<Renderer>();

            switch (enemyInstance.GetComponent<Enemy>().EnemyType)
            {
                case EnemyType.MeleeEnemy:
                    rend.material.color = Color.yellow;
                    break;

                case EnemyType.DistantEnemy:
                    rend.material.color = Color.blue;
                    var spiderPercentSpawn = UnityEngine.Random.Range(0, 100);

                    if (spiderPercentSpawn < _spiderProbableInstance * 100)
                    {
                        SetSpider(enemyInstance);
                    }
                    break;

                default:
                    break;
            }
        }


        private void SetSpider(GameObject enemyInstance)
        {

            var rend = enemyInstance.GetComponent<Renderer>();
            rend.enabled = false;
            enemyInstance.GetComponent<NavMeshAgent>().radius = _spawnRadius;
            UnityEngine.Object.Instantiate(_spiderPrefab, enemyInstance.transform);
        }


        private GameObject SpawnEnemy()
        {
            GameObject sceneInstance = _container.InstantiatePrefab(_prefab);
            sceneInstance.transform.position = _spawnTransform.position;
            return sceneInstance;
        }


        private void SetTypeEnemy(GameObject enemyInstance)
        {

            if ((_numberMeleeEnemy_cnt--) > 0)
            {
                
                enemyInstance.GetComponent<Enemy>().EnemyType = EnemyType.MeleeEnemy;
            }
            else if ((_numberDistantEnemy_cnt--) > 0)
            {
              
                enemyInstance.GetComponent<Enemy>().EnemyType = EnemyType.DistantEnemy;
            }
        }


        private void SetSystems(GameObject enemyInstance)
        {
            var enemy = enemyInstance.GetComponent<IEnemy>();
            enemy.SetSystems(CreateSystems(enemyInstance));
            enemy.SetComponents(CreateComponents(), _rangeRadiusRange);
        }


        private void GetPlayerTransform(GameObject enemyInstance)
        => _playerTransform = enemyInstance.GetComponent<Enemy>().PlayerTransform;
        

        private IEnemyComponentsStore CreateComponents()
        {

            EnemyAttackComponent attackable = new EnemyAttackComponent();
            EnemyPriceComponent enemyPrice = new EnemyPriceComponent(_goldDropRate);

            return new EnemyComponentsStore(attackable, enemyPrice);
        }


        private List<ISystem> CreateSystems(GameObject enemyInstance)
        {
            var systems = new List<ISystem>();

            switch (GetEnemyType(enemyInstance))
            {
                case EnemyType.MeleeEnemy:
                    systems.Add(new EnemyMovementSystem(2));
                    systems.Add(new EnemyDamageSystem());
                    systems.Add(new EnemyMeleeAttackSystem());
                    break;

                case EnemyType.DistantEnemy:
                    systems.Add(new EnemyMovementSystem(10));
                    systems.Add(new EnemyDamageSystem());
                    systems.Add(new EnemyDistantAttackSystem());
                    break;

                case EnemyType.DistantMeleeEnemy:
                    systems.Add(new EnemyMovementSystem(10));
                    systems.Add(new EnemyDamageSystem());
                    systems.Add(new EnemyMeleeAttackSystem());
                    systems.Add(new EnemyDistantAttackSystem());
                    break;

                case EnemyType.Kamikaze:
                    systems.Add(new EnemyMovementSystem(1));
                    systems.Add(new EnemyDamageSystem());
                    systems.Add(new EnemyKamikazeAttackSystem());
                    break;

                default:
                    break;
            }
            return systems;
        }


        private EnemyType GetEnemyType(GameObject enemyInstance) => enemyInstance.GetComponent<IEnemy>().EnemyType;
         

        private void TrySpawnEnemy()
        {
            if (activeEnemyCount < _poolSize)
            {
                GameObject enemyInstance = _enemyPool.Get();
                var enemy = enemyInstance.GetComponent<Enemy>();
                
                SetEnemyPosition(enemyInstance);
                SetDeadFlagInTrue(enemy);

                enemy.ComponentsStore.Attackable.IsDeadFlag.Subscribe(isDead =>
                {
                    if (isDead)
                    {
                        AddExpToPlayer(enemyInstance);
                        AddGoldToPlayer(enemyInstance);
                        ReturnEnemyToPool(enemyInstance);
                    }
                });
                enemy.gameObject.SetActive(true);

                activeEnemyCount++;
            }
        }


        private void AddGoldToPlayer(GameObject enemyInstance)
        {
            
            var eneny = enemyInstance.GetComponent<Enemy>();
            int goldValue = eneny.ComponentsStore.EnemyPrice.GetGoldValue();

            if (goldValue > 0)
                _componentsPlayer.GoldWallet.AddGold(goldValue);
        }


        private void AddExpToPlayer(GameObject enemyInstance)
        {
            var eneny = enemyInstance.GetComponent<Enemy>();
            var ExpValue = eneny.ComponentsStore.EnemyPrice.GetExperienceValue();
            _componentsPlayer.ExperienceHandle.AddExperience(ExpValue);
        }


        private void SetDeadFlagInTrue(Enemy enemy) => enemy.ComponentsStore.Attackable.IsDeadFlag.Value = false;
        

        private void SetEnemyPosition(GameObject enemyInstance)
         => enemyInstance.transform.position = _playerTransform.position + GetCircleIntersectionCoordinates() + Vector3.down;
         

        private Vector3 GetCircleIntersectionCoordinates()
        {
            float randomAngle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
            return new Vector3(Mathf.Cos(randomAngle) * _spawnRadius, 0f, Mathf.Sin(randomAngle) * _spawnRadius);
        }


    }
}