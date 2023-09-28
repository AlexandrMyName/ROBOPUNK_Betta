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

    public class EnemySpawnerController
    {

        [Inject] private DiContainer _container;
        [Inject(Id = "PlayerComponents")] private IComponentsStore _componentsPlayer;

        private EnemyDataConfig _enemyDataConfig;
        private EnemySpawnerDataConfig _enemySpawnerDataConfig;
        private List<EnemySpawner> _enemySpawners;


        internal EnemySpawnerController(EnemyDataConfig enemyDataConfig, EnemySpawnerDataConfig enemySpawnerDataConfig)
        {
            _enemyDataConfig = enemyDataConfig;
            _enemySpawnerDataConfig = enemySpawnerDataConfig;

            Init();
        }



        internal void StartSpawnProcess()
        {
            
        }


        internal void StopSpawnProcess()
        {

        }


        private void Init()
        {
            for (int i = 0; i < _enemyDataConfig.Configs.Count; i++)
            {
                EnemySpawner spawner = CreateSpawner(_enemyDataConfig.Configs[i]);
                _enemySpawners.Add(spawner);
            }
        }


        private EnemySpawner CreateSpawner(EnemyConfig enemyConfig)
        {

            return null;
        }


    }
}