using System;
using UnityEngine;
using UniRx;
using Zenject;
using Abstracts;
using User;
using Core;

namespace DI.Spawn
{

    public class EnemySpawnerController
    {
        private EnemySpawnerDataConfig _enemySpawnerDataConfig;
        private DiContainer _diContainer;
        private IDisposable _waveDisposable;
        private EnemySpawner _enemySpawner;
        private Transform _playerPosition;
        private IExperienceHandle _experienceHandle;
        private IGoldWallet _goldWallet;

        private int _waveNumber;
        private bool _loopWave;
        private float _respawnWaveDelay;

        private int _waveCountInLoop;

        internal EnemySpawnerController(
            EnemySpawnerDataConfig enemySpawnerDataConfig, 
            DiContainer diContainer,
            Transform playerPosition, 
            IGoldWallet goldWallet,
            IExperienceHandle experienceHandle)
        {
            _enemySpawnerDataConfig = enemySpawnerDataConfig;
            _diContainer = diContainer;
            _waveNumber = _enemySpawnerDataConfig.Wave.Count;
            _loopWave = _enemySpawnerDataConfig.loop;
            _respawnWaveDelay = _enemySpawnerDataConfig.respawnWaveDelay;
            _playerPosition = playerPosition;
            _goldWallet = goldWallet;
            _experienceHandle = experienceHandle;
        }


        internal void StartSpawnProcess()
        {

            _waveDisposable = Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(_respawnWaveDelay))
                .Where(_ => (_loopWave ? LoppWaveTrue() : LoopWaveFalse()))
                .Subscribe(cnt => CallingWave((int)cnt));

            int numberEnemiesInScene = 0;
            foreach (var wave in _enemySpawnerDataConfig.Wave)
            {
                numberEnemiesInScene += wave.numberEnemiesInWave;
            }

            _enemySpawner = new EnemySpawner( _diContainer, _playerPosition, numberEnemiesInScene, _experienceHandle, _goldWallet);
        }



        internal void StopSpawnProcess()
        {
            _waveDisposable.Dispose();
        }


        private void CallingWave(int cnt)
        {
            _enemySpawner.StartSpawnWave(_enemySpawnerDataConfig.Wave[_waveCountInLoop-1], cnt);
        }



        private bool LoopWaveFalse()
        {
            var value = (_waveCountInLoop < _waveNumber);
            _waveCountInLoop++;

            return value;
        }


        private bool LoppWaveTrue()
        {
            if (_waveCountInLoop >= _waveNumber)
                _waveCountInLoop = 0;

            _waveCountInLoop++;

            return true;
        }



    }
}