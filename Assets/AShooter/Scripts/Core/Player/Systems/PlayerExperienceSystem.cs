using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Core
{

    public class PlayerExperienceSystem : BaseSystem , IDisposable
    {
        
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private IExperienceView _experienceView;
        private IExperienceHandle _experienceHandle;

        private float _requiredExperienceForNextLevel;
        private float _progressRate;


        protected override void Awake(IGameComponents components)
        {
            var componentsStore = components.BaseObject.GetComponent<IPlayer>().ComponentsStore;
            _experienceView = componentsStore.Views.Experience;
            _experienceHandle = componentsStore.ExperienceHandle;
            _requiredExperienceForNextLevel = componentsStore.LevelProgress.RequiredExperienceForNextLevel;
            _progressRate = componentsStore.LevelProgress.ProgressRate;

            SubscribeToExperienceChanges();
            SubscribeToLevelChanges();

            _experienceView.Show();
            UpdateDisplay(_experienceHandle.CurrentExperience.Value, _experienceHandle.CurrentLevel.Value, _requiredExperienceForNextLevel);
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
            _experienceHandle.CurrentLevel.Value++;
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

