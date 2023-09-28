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
        [Inject(Id = "PlayerPosition")] private Vector3 _playerPosition;

        private EnemySpawnerDataConfig _enemySpawnerDataConfig;
        private IDisposable _waveDisposable;
        private EnemySpawner _enemySpawner;

        private int _waveNumber;
        private bool _loopWave;
        private float _respawnWaveDelay;

        private int _waveCount;

        internal EnemySpawnerController(EnemySpawnerDataConfig enemySpawnerDataConfig)
        {
            _enemySpawnerDataConfig = enemySpawnerDataConfig;

            _waveNumber = _enemySpawnerDataConfig.Wave.Count;
            Debug.Log($"___Wave number {_waveNumber}___");
            _loopWave = _enemySpawnerDataConfig.loop;
            _respawnWaveDelay = _enemySpawnerDataConfig.respawnWaveDelay;
        }


        internal void StartSpawnProcess()
        {
            Debug.Log("___Start Spawn Process___");
            _waveDisposable = Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(_respawnWaveDelay))
                .Where(_ => (_loopWave ? LoppWaveTrue() : LoopWaveFalse()))
                .Subscribe(_ => CallingWave());

            _enemySpawner = new EnemySpawner(_container, _playerPosition);
        }



        internal void StopSpawnProcess()
        {
            _waveDisposable.Dispose();
        }


        private void CallingWave()
        {
            Debug.Log($"____enemySpawnerDataConfig.Wave[{_waveCount-1}] {_enemySpawnerDataConfig.Wave[_waveCount - 1]}___");
            _enemySpawner.StartSpawnProcess(_enemySpawnerDataConfig.Wave[_waveCount-1]);
        }



        private bool LoopWaveFalse()
        {
            var value = (_waveCount < _waveNumber);
            _waveCount++;

            return value;
        }


        private bool LoppWaveTrue()
        {
            if (_waveCount >= _waveNumber)
                _waveCount = 0;

            _waveCount++;

            return true;
        }



    }
}