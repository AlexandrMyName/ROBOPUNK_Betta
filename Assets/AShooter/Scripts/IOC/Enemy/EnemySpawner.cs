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

        private float _respawnEnemyInWaveDelay;
        private float _spawnRadiusRelativeToPlayer;
        private List<EnemyConfig> _enemyConfigs;
        private int _numberEnemiesInWave;

        private IDisposable _spawnDisposable;
        private Vector3 _targetPosition;
        private DiContainer _diContainer;

        private ObjectPool<GameObject> _pool;
        private Dictionary<EnemyType, ObjectPool<GameObject>> _enemyTypeObjectPolPairs;
        private List<GameObject> _enemyPrototypes;
        private List<int> _enemyNumberPrototypes;

        private int _enemy—ounter;


        public EnemySpawner(DiContainer diContainer, Vector3 targetPosition)
        {
            _diContainer = diContainer;
            _targetPosition = targetPosition;
        }


        internal void StartSpawnProcess(EnemyWaveConfig enemyWaveConfig)
        {
            _numberEnemiesInWave = 0;
            _enemyPrototypes = new List<GameObject>();
            _enemyNumberPrototypes = new List<int>();
            _enemy—ounter = 0;
            //_pool.Clear();
            //_spawnDisposable.Dispose();


            _respawnEnemyInWaveDelay = enemyWaveConfig.respawnEnemyInWaveDelay;
            _spawnRadiusRelativeToPlayer = enemyWaveConfig.spawnRadiusRelativeToPlayer;
            _enemyConfigs = enemyWaveConfig.Enemy;
            
            foreach (var item in _enemyConfigs)
            {
                _numberEnemiesInWave += item.numberEnemiesInWave;
                _enemyPrototypes.Add(BuildPrototypeEnemy(item));
                _enemyNumberPrototypes.Add(item.numberEnemiesInWave);
            }

            if (_pool == null)
                _pool = new ObjectPool<GameObject>(
                    createFunc: () => CreateEnemy(),
                    actionOnGet: (obj) => OnTakeFromPool(obj),
                    actionOnRelease: (obj) => OnReturnedToPool(obj),
                    actionOnDestroy: (obj) => OnDestroyPoolObject(obj),
                    collectionCheck: false,
                    defaultCapacity: _numberEnemiesInWave,
                    maxSize: _numberEnemiesInWave);
            if (_spawnDisposable == null)
                _spawnDisposable = Observable
                    .Interval(TimeSpan.FromSeconds(_respawnEnemyInWaveDelay))
                    .Where(_ => { return (_enemy—ounter < _numberEnemiesInWave); })
                    .Subscribe(cnt => { Debug.Log($"_enemy—ounter -> {_enemy—ounter}"); _enemy—ounter++; _pool.Get(); });

            
        }


        private GameObject BuildPrototypeEnemy(EnemyConfig item)
        {
            var prefab = item.prefab;

            var enemy = prefab.GetComponent<Enemy>();
            enemy.EnemyType = item.enemyType;

            enemy.SetSystems(CreateSystems(item));

            enemy.SetComponents(CreateComponents(item));

            return prefab;
        }


        private List<ISystem> CreateSystems(EnemyConfig item)
        {
            var systems = new List<ISystem>();

            switch (item.enemyType)
            {
                case EnemyType.MeleeEnemy:
                    systems.Add(new EnemyMovementSystem(item.attackDistance,_targetPosition));
                    systems.Add(new EnemyDamageSystem(item.maxHealth));
                    systems.Add(new EnemyMeleeAttackSystem(item.attackDistance, item.attackFrequency));
                    break;

                case EnemyType.DistantEnemy:
                    systems.Add(new EnemyMovementSystem(item.attackDistance, _targetPosition));
                    systems.Add(new EnemyDamageSystem(item.maxHealth));
                    systems.Add(new EnemyDistantAttackSystem(_targetPosition, item.attackFrequency));
                    break;

                default:
                    systems.Add(new EnemyMovementSystem(item.attackDistance, _targetPosition));
                    systems.Add(new EnemyDamageSystem(item.maxHealth));
                    systems.Add(new EnemyMeleeAttackSystem(item.attackDistance, item.attackFrequency));
                    break;
            }
            return systems;
        }


        private IEnemyComponentsStore CreateComponents(EnemyConfig item)
        {
            EnemyAttackComponent attackable = new EnemyAttackComponent(item.maxHealth, item.maxAttackDamage, item.attackDistance);
            EnemyPriceComponent enemyPrice = new EnemyPriceComponent(item.goldDropRate, item.goldValueRange, item.experienceRange);

            return new EnemyComponentsStore(attackable, enemyPrice);
        }


        private GameObject CreateEnemy()
        {

            //var x =_enemyPrototypes[i];
            //_enemyNumberPrototypes[i];

            var enemyGo = _enemyPrototypes[0];
            //enemyGo = _diContainer.InstantiatePrefab(enemyGo);

            return enemyGo;
        }



        private void OnTakeFromPool(GameObject obj)
        {

        }


        private void OnDestroyPoolObject(GameObject obj)
        {

        }


        private void OnReturnedToPool(GameObject obj)
        {

        }


        internal void StopSpawning()
        {
            _spawnDisposable.Dispose();
        }


        internal void ReturnEnemyToPool(GameObject enemyInstance)
        {
            enemyInstance.SetActive(false);

        }


        //private void SetSpider(GameObject enemyInstance)
        //{
        //
        //    var rend = enemyInstance.GetComponent<Renderer>();
        //    rend.enabled = false;
        //    enemyInstance.GetComponent<NavMeshAgent>().radius = _spawnRadius;
        //    UnityEngine.Object.Instantiate(_spiderPrefab, enemyInstance.transform);
        //}


        //private void TrySpawnEnemy()
        //{
        //    if (activeEnemyCount < _poolSize)
        //    {
        //        GameObject enemyInstance = _enemyPool.Get();
        //        var enemy = enemyInstance.GetComponent<Enemy>();
        //        
        //        SetEnemyPosition(enemyInstance);
        //        SetDeadFlagInFalse(enemy);
        //
        //        enemy.ComponentsStore.Attackable.IsDeadFlag.Subscribe(isDead =>
        //        {
        //            if (isDead)
        //            {
        //                AddExpToPlayer(enemyInstance);
        //                AddGoldToPlayer(enemyInstance);
        //                ReturnEnemyToPool(enemyInstance);
        //            }
        //        });
        //        enemy.gameObject.SetActive(true);
        //
        //        activeEnemyCount++;
        //    }
        //}


        //private void AddGoldToPlayer(GameObject enemyInstance)
        //{
        //    
        //    var eneny = enemyInstance.GetComponent<Enemy>();
        //    int goldValue = eneny.ComponentsStore.EnemyPrice.GetGoldValue();
        //
        //    if (goldValue > 0)
        //        _componentsPlayer.GoldWallet.AddGold(goldValue);
        //}


        //private void AddExpToPlayer(GameObject enemyInstance)
        //{
        //    var eneny = enemyInstance.GetComponent<Enemy>();
        //    var ExpValue = eneny.ComponentsStore.EnemyPrice.GetExperienceValue();
        //    _componentsPlayer.ExperienceHandle.AddExperience(ExpValue);
        //}


        private void SetDeadFlagInFalse(Enemy enemy) => enemy.ComponentsStore.Attackable.IsDeadFlag.Value = false;
        

        //private void SetEnemyPosition(GameObject enemyInstance)
        // => enemyInstance.transform.position = _playerTransform.position + GetCircleIntersectionCoordinates() + Vector3.down;
         

        //private Vector3 GetCircleIntersectionCoordinates()
        //{
        //    float randomAngle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
        //    return new Vector3(Mathf.Cos(randomAngle) * _spawnRadius, 0f, Mathf.Sin(randomAngle) * _spawnRadius);
        //}


    }
}