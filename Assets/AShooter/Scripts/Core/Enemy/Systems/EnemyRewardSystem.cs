using Abstracts;
using AShooter.Scripts.User.Views;
using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using User;
using User.Components.Repository;
using Zenject;
using static Unity.VisualScripting.Member;


namespace Core
{

    public class EnemyRewardSystem : BaseSystem, IDisposable
    {

        private List<IDisposable> _disposables = new();
        private IExperienceHandle _experienceHandle;
        private IGoldWallet _goldWallet;
        private IPlayerStats _playerStats;
        
        private ReactiveProperty<bool> _isRewardReadyFlag;
        private Enemy _enemy;

        private Transform _targetPosition;


        public EnemyRewardSystem(IExperienceHandle experienceHandle, IGoldWallet goldWallet, IPlayerStats playerStats, Transform targetPosition)
        {
            _experienceHandle = experienceHandle;
            _goldWallet = goldWallet;
            _playerStats = playerStats;
            _targetPosition = targetPosition;
        }


        protected override void Awake(IGameComponents components)
        {
            _enemy = components.BaseObject.GetComponent<Enemy>();
            _isRewardReadyFlag = _enemy.ComponentsStore.Attackable.IsRewardReadyFlag;
        }


        protected override void OnEnable()
        {
            _disposables.Add(_enemy.ComponentsStore.Attackable.IsRewardReadyFlag.Subscribe(GiveThePlayerReward));
        }


        private void GiveThePlayerReward(bool deadFlag)
        {
            if (deadFlag)
            {
                AddGoldToPlayer();
                AddExperienceToPlayer();
            }

            _isRewardReadyFlag.Value = false;
        }


        private void AddGoldToPlayer()
        {
            int goldValue = _enemy.ComponentsStore.EnemyPrice.GetGoldValue();

            if (goldValue > 0)
                _goldWallet.AddGold(goldValue);
        }


        private void AddExperienceToPlayer()
        {
            float experienceValue = _enemy.ComponentsStore.EnemyPrice.GetExperienceValue();
            _experienceHandle.AddExperience(experienceValue);
            _playerStats.AddMetaExperience(experienceValue);

            ShowExperienceBall();
        }


        private void ShowExperienceBall() 
        {
            var experienceBall = GameObject.Instantiate(_experienceHandle.ExperienceBall, _enemy.transform.position, Quaternion.identity);
            float progress = 0.05f;

            Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (experienceBall)
                {
                    var distance = Vector3.Distance(experienceBall.transform.position, _targetPosition.position);

                    if (distance >= 0.8f)
                    {
                        progress += Time.deltaTime * 0.05f;
                        experienceBall.transform.position = Vector3.Lerp(experienceBall.transform.position, _targetPosition.position, progress);
                    }
                    else
                        GameObject.Destroy(experienceBall.gameObject);
                }
            }).AddTo(_disposables);
        }


        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }


        
    }
}