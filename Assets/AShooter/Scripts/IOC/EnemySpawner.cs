using System;
using UnityEngine;
using UniRx;
using Core;
using Zenject;

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
            GameObject enemyInstance = Spawn();
            _playerTransform = enemyInstance.GetComponent<Enemy>().playerTransform;
            return enemyInstance;
        }
        
        
        public GameObject Spawn()
        {
            GameObject sceneInstance = _container.InstantiatePrefab(_prefab);
            sceneInstance.transform.position = _spawnTransform.position;
            return sceneInstance;
        }

        
        private void TrySpawnEnemy()
        {
            if (activeEnemyCount < poolSize)
            {
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