using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;
using User.Components.Repository;
using Zenject;


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


        public EnemyRewardSystem(IExperienceHandle experienceHandle, IGoldWallet goldWallet, IPlayerStats playerStats)
        {
            _experienceHandle = experienceHandle;
            _goldWallet = goldWallet;
            _playerStats = playerStats;
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
        }


        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }


        
    }
}