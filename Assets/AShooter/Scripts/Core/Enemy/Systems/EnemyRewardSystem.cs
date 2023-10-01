using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;


namespace Core
{

    public class EnemyRewardSystem : BaseSystem
    {

        private List<IDisposable> _disposables = new();
        private IExperienceHandle _experienceHandle;
        private IGoldWallet _goldWallet;
        private ReactiveProperty<bool> _isRewardReadyFlag;
        private Enemy _enemy;


        public EnemyRewardSystem(IExperienceHandle experienceHandle, IGoldWallet goldWallet)
        {
            _experienceHandle = experienceHandle;
            _goldWallet = goldWallet;
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
        }


        private void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }


        protected override void OnDestroy()
        {
            Dispose();
        }


    }
}