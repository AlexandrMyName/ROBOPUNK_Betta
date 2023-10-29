using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{

    public class PlayerExperienceSystem : BaseSystem , IDisposable
    {
        
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private IExperienceView _experienceView;
        private IExperienceHandle _experienceHandle;
        private IRewardMenuView _rewardMenuUI;
        private GameObject _baseObject;

        private ParticleSystem _levelUpEffect;
        private float _requiredExperienceForNextLevel;
        private float _progressRate;


        protected override void Awake(IGameComponents components)
        {
            _baseObject = components.BaseObject;
            var componentsStore = _baseObject.GetComponent<IPlayer>().ComponentsStore;
            _experienceView = componentsStore.Views.Experience;
            _experienceHandle = componentsStore.ExperienceHandle;
            _requiredExperienceForNextLevel = componentsStore.LevelProgress.RequiredExperienceForNextLevel;
            _progressRate = componentsStore.LevelProgress.ProgressRate;
            _rewardMenuUI = componentsStore.Views.RewardMenu;

            AddLevelUpEffectToPlayerObject();
            SubscribeToExperienceChanges();
            SubscribeToLevelChanges();

            _experienceView.Show();
            UpdateDisplay(_experienceHandle.CurrentExperience.Value, _experienceHandle.CurrentLevel.Value, _requiredExperienceForNextLevel);
        }


        private void AddLevelUpEffectToPlayerObject()
        {
            var levelUpEffectObject = Object.Instantiate(new GameObject("LevelUpEffect"), _baseObject.transform);
            var effectPrefrab = Object.Instantiate(_experienceHandle.LevelUpEffect, levelUpEffectObject.transform);

            _levelUpEffect = effectPrefrab;
            _levelUpEffect.Stop();
        }


        private void SubscribeToExperienceChanges()
        {
            _disposables.Add(_experienceHandle.CurrentExperience.Subscribe(OnExperienceChanged));
        }


        private void SubscribeToLevelChanges()
        {
            _disposables.Add(_experienceHandle.CurrentLevel.Subscribe(OnLevelChanged));
        }


        private void OnLevelChanged(int valueLevel)
        {
            UpdateDisplay(_experienceHandle.CurrentExperience.Value, valueLevel, _requiredExperienceForNextLevel);
        }


        private void OnExperienceChanged(float valueExperience)
        {
            if (valueExperience >= _requiredExperienceForNextLevel)
            {
                MakeLevelUp();

                _requiredExperienceForNextLevel = _requiredExperienceForNextLevel * _progressRate;
            }

            UpdateDisplay(valueExperience, _experienceHandle.CurrentLevel.Value, _requiredExperienceForNextLevel);
        }


        private void MakeLevelUp()
        {
            _levelUpEffect.Play();

            _experienceHandle.CurrentLevel.Value++;

            Observable.Timer(TimeSpan.FromSeconds(3))
                .Subscribe(_ =>
                {
                    _levelUpEffect.Stop();
                }).AddTo(_baseObject);
        }


        private void UpdateDisplay(float valueCurrentExperience, int valueLvl, float valueProgressExperience)
        {
            _experienceView.ChangeDisplay(valueCurrentExperience, valueLvl, valueProgressExperience);
        }


        public void Dispose() 
        {
            _disposables.ForEach(disposable => disposable.Dispose());
            _disposables.Clear();
        } 


    }
}

