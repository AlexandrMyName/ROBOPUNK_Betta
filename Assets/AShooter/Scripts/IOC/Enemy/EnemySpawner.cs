using System;
using UnityEngine;
using UniRx;
using Core;
using Zenject;
using System.Collections.Generic;
using Abstracts;
using Core.DTO;
using UnityEngine.Pool;
using User;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem.iOS;
using Unity.VisualScripting;

namespace DI.Spawn
{

    public class EnemySpawner
    {

        private float _respawnEnemyInWaveDelay;
        private float _spawnRadiusRelativeToPlayer;
        private List<EnemyConfig> _enemyConfigs;
        private int _numberEnemiesInWave;
        private int _numberEnemiesInScene;

        private IDisposable _spawnDisposable;
        private Transform _targetPosition;
        private DiContainer _diContainer;
        private IExperienceHandle _experienceHandle;
        private IGoldWallet _goldWallet;
        private IPlayerStats _playerStats;
        private IWeaponStorage _weaponStorage;

        private ObjectPool<GameObject> _pool;
        private List<float> _enemySpawnWeight;
        private List<IDisposable> _disposables;

        private GameObject _waves;
        private GameObject _enemyViews_Prefab;
        private List<GameObject> _waveGameObjects;
        private int _waveCount;


        public EnemySpawner(
            DiContainer diContainer, 
            Transform targetPosition, 
            int maxNumberEnemy,
            IExperienceHandle experienceHandle,  
            IGoldWallet goldWallet,
            GameObject enemyViews,
            IPlayerStats playerStats,    
            IWeaponStorage weaponStorage){
            ////[CONSTRUCT]\\\\

            _enemyViews_Prefab = enemyViews;
            _disposables = new();

            _diContainer = diContainer;
            _targetPosition = targetPosition;

            _experienceHandle = experienceHandle;
            _goldWallet = goldWallet;
            _playerStats = playerStats;
            _weaponStorage = weaponStorage;

            _disposables.Add(
                _pool = new ObjectPool<GameObject>(
                    createFunc: () => RandomEnemyCreation(),
                    actionOnGet: (obj) => OnTakeFromPool(obj),
                    actionOnRelease: (obj) => OnReturnedToPool(obj),
                    actionOnDestroy: (obj) => OnDestroyPoolObject(obj),
                    collectionCheck: false,
                    defaultCapacity: _numberEnemiesInWave,
                    maxSize: maxNumberEnemy)
                );

            _waves = new GameObject("___WAVES_of_ENEMIES___");
            _waveGameObjects = new List<GameObject>();
        }


        public void StartSpawnWave(EnemyWaveConfig enemyWaveConfig, int waveCount)
        {
            Dispose();

            _numberEnemiesInWave = 0;
            _numberEnemiesInScene = 0;
            _enemySpawnWeight = new List<float>();

            _waveCount = waveCount;

            _respawnEnemyInWaveDelay = enemyWaveConfig.respawnEnemyInWaveDelay;
            _spawnRadiusRelativeToPlayer = enemyWaveConfig.spawnRadiusRelativeToPlayer;
            _enemyConfigs = enemyWaveConfig.Enemy;
            _numberEnemiesInWave = enemyWaveConfig.numberEnemiesInWave;
            _numberEnemiesInScene = enemyWaveConfig.numberEnemiesInScene;

            foreach (var item in _enemyConfigs) {
                _enemySpawnWeight.Add(item.probabilityWeigh);
            }

            _disposables.Add(
                _spawnDisposable = Observable
                    .Interval(TimeSpan.FromSeconds(_respawnEnemyInWaveDelay))
                    .Where(_ => {  return (_pool.CountActive < _numberEnemiesInScene); })
                    .Subscribe(_ => { _pool.Get(); })
                );

            var goWave = new GameObject($"Wave_[{waveCount}]");
            goWave.transform.SetParent(_waves.transform);
            _waveGameObjects.Add(goWave);
        }


        private GameObject RandomEnemyCreation()
        {
            var enemyConfig = RandomEnemyConfig();
            var enemyGo = EnemyCreation(enemyConfig);

            return enemyGo;
        }


        private EnemyConfig RandomEnemyConfig()
        {
            float[] probSum = new float[_enemySpawnWeight.Count];

            probSum[0] = _enemySpawnWeight[0];
            for (int i = 1; i < _enemySpawnWeight.Count; i++)
            {
                probSum[i] = probSum[i - 1] + _enemySpawnWeight[i];
            }


            float randomValue = Random.Range(0f, 1f);
            if (randomValue <= probSum[0])
                return _enemyConfigs[0];

            for (int i = 1; i < _enemyConfigs.Count; i++)
            {
                if (randomValue > probSum[i - 1] && randomValue <= probSum[i])
                {
                    return _enemyConfigs[i];
                }
            }

            return _enemyConfigs[0];
        }


        private GameObject EnemyCreation(EnemyConfig item)
        {
            var prefab = _diContainer.InstantiatePrefab(item.prefab, _waveGameObjects[_waveCount].transform);
            ViewsCreation(prefab.transform);
            var enemy = prefab.GetComponent<Enemy>();
            enemy.EnemyType = item.enemyType;
            enemy.SetComponents(CreateComponents(item));
            enemy.SetSystems(CreateSystems(item));

            return prefab;
        }


        private void ViewsCreation(Transform parent)
        {

           var viewsInstance =  GameObject.Instantiate(_enemyViews_Prefab, parent, false);
           // viewsInstance.transform.localScale = Vector3.one;

            viewsInstance.transform.localPosition = Vector3.zero + Vector3.up * 1.5f;
            viewsInstance.GetComponent<IEnemyViews>().InitViews();
        }


        private List<ISystem> CreateSystems(EnemyConfig item)
        {

            var systems = new List<ISystem>();
            systems.Add(new EnemyRewardSystem(_experienceHandle, _goldWallet, _playerStats, _targetPosition));
            systems.Add(new EnemyDamageSystem(item.maxHealth,item.maxProtection));
            systems.Add(new EnemyMovementSystem(_targetPosition));

            switch (item.enemyType)
            {
                case EnemyType.MeleeEnemy:
                    systems.Add(new EnemyMeleeAttackSystem());
                    break;

                case EnemyType.DistantEnemy:
                    systems.Add(new EnemyDistantAttackSystem(_targetPosition));
                    break;

                default:
                    systems.Add(new EnemyMeleeAttackSystem());
                    break;
            }
            return systems;
        }


        private IEnemyComponentsStore CreateComponents(EnemyConfig item)
        {
            EnemyAttackComponent attackable = new EnemyAttackComponent(item.maxHealth, item.maxProtection, item.maxAttackDamage, item.attackDistance, item.attackFrequency);
            EnemyPriceComponent enemyPrice = new EnemyPriceComponent(item.goldDropRate, item.goldValueRange, item.experienceRange);

            return new EnemyComponentsStore(attackable, enemyPrice, _weaponStorage);
        }

     
        private void OnTakeFromPool(GameObject obj)
        {
            SetDeadFlagInFalse(obj);
            SetEnemyPosition(obj);

            var enemy = obj.GetComponent<Enemy>();
            enemy.ComponentsStore
                .Attackable
                .IsDeadFlag
                .Subscribe(isDead => { CheckingWaveMembership(isDead, obj); });
              //  .AddTo(_disposables);

            obj.SetActive(true);
        }


        private void CheckingWaveMembership(bool isDead, GameObject obj)
        {
            //var waveGo = obj.transform.parent.gameObject;
            //var lastWaveGo = _waveGameObjects[_waveGameObjects.Count - 1];

            if (isDead) {
                //if (lastWaveGo.name == waveGo.name)
                    _pool.Release(obj);
                //else
                //    Object.Destroy(obj);
            }

            //foreach (var item in _waveGameObjects)
            //{
            //    if (item.transform.childCount == 0)
            //    {
            //        _waveGameObjects.Remove(item);
            //    }
            //}
        }


        private void OnDestroyPoolObject(GameObject obj)
        {
            var enemy = obj.GetComponent<Enemy>();
            enemy.ComponentsStore.Attackable.IsDeadFlag.Dispose();
        }


        private void OnReturnedToPool(GameObject obj)
        {
            var enemy = obj.GetComponent<Enemy>();
            enemy.ComponentsStore.Attackable.IsDeadFlag.Dispose();

            obj.SetActive(false);
        }


        internal void StopSpawning()
        {
            _spawnDisposable.Dispose();
        }

         
        public void Dispose()
        {

            _disposables?.ForEach(disposable => { if (disposable != null) disposable.Dispose(); });
        }


        private void SetDeadFlagInFalse(GameObject enemy) {
            enemy.GetComponent<Enemy>().ComponentsStore.Attackable.IsDeadFlag = new ReactiveProperty<bool>(false);
        } 
        

        private void SetEnemyPosition(GameObject enemyInstance) {

            enemyInstance.transform.position = _targetPosition.position + GetCircleIntersectionCoordinates() + Vector3.down;
        }
        

        private Vector3 GetCircleIntersectionCoordinates()
        {
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);
            return new Vector3(Mathf.Cos(randomAngle) * _spawnRadiusRelativeToPlayer, 0f, Mathf.Sin(randomAngle) * _spawnRadiusRelativeToPlayer);
        }


    }
}