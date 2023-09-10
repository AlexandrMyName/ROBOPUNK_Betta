using System;
using UnityEngine;
using UniRx;
using Core;
using Zenject;
using System.Collections.Generic;
using Abstracts;

namespace DI.Spawn
{

    public class EnemySpawner : MonoBehaviour
    {

        [Inject] private DiContainer _container;

        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _spawnTransform;

        [SerializeField] private float respawnDelay = 1f;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private float spawnRadius = 5f;

        private GameObjectPool enemyPool;
        private IDisposable spawnDisposable;
        private Transform _playerTransform;

        private int activeEnemyCount = 0;

        internal void StartSpawnProcess()
        {
            enemyPool = new GameObjectPool(() => CreateEnemy(), poolSize);
            spawnDisposable = Observable
                .Interval(TimeSpan.FromSeconds(respawnDelay))
                .TakeUntilDestroy(this)
                .Subscribe(_ => TrySpawnEnemy());
        }


        private GameObject CreateEnemy()
        {
            Debug.Log("1");
            GameObject enemyInstance = Spawn();

            var enemy = enemyInstance.GetComponent<Enemy>();
            _playerTransform = enemy.PlayerTransform;

            return enemyInstance;
        }


        private GameObject Spawn()
        {
            Debug.Log("2");
            GameObject sceneInstance = _container.InstantiatePrefab(AddingSystems(_prefab));
            sceneInstance.transform.position = _spawnTransform.position;
            return sceneInstance;
        }


        private GameObject AddingSystems(GameObject prefab)
        {
            Debug.Log("3");
            var clonePrefab = Instantiate(prefab, this.transform, false);
            clonePrefab.GetComponent<Enemy>().SetSystems(createSystem());

            return clonePrefab;
        }


        private List<ISystem> createSystem()
        {
            Debug.Log("4");
            var systems = new List<ISystem>();

            systems.Add(new EnemyMovementSystem(2));
            systems.Add(new EnemyDamageSystem());
            systems.Add(new EnemyMeleeAttackSystem());

            return systems;
        }


        private void TrySpawnEnemy()
        {
            if (activeEnemyCount < poolSize)
            {
                Debug.Log("6");
                GameObject enemyInstance = enemyPool.Get();

                var enemy = enemyInstance.GetComponent<Enemy>();
                enemyInstance.transform.position = _playerTransform.position + GetCircleIntersectionCoordinates() + Vector3.down;
                enemy.IsDeadFlag.Value = false;

                enemy.IsDeadFlag.Subscribe(isDead =>
                {
                    if (isDead)
                    {
                        ReturnEnemyToPool(enemyInstance);
                    }
                });
                enemy.gameObject.SetActive(true);

                activeEnemyCount++;
            }
        }


        internal void StopSpawning()
        {
            spawnDisposable.Dispose();
        }


        internal void ReturnEnemyToPool(GameObject enemyInstance)
        {
            enemyInstance.SetActive(false);

            enemyPool.Return(enemyInstance);
            activeEnemyCount--;
        }


        private Vector3 GetCircleIntersectionCoordinates()
        {
            float randomAngle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
            return new Vector3(Mathf.Cos(randomAngle) * spawnRadius, 0f, Mathf.Sin(randomAngle) * spawnRadius);
        }

    }

}