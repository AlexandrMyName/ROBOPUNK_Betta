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

        private float requiredExperienceForNextLevel = 10f;
        private float progressRate = 2.0f;


        protected override void Awake(IGameComponents components)
        {
            _experienceView = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.Experience;
            _experienceHandle = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.ExperienceHandle;

            SubscribeToExperienceChanges();
            SubscribeToLevelChanges();

            _experienceView.Show();
            UpdateDisplay(_experienceHandle.CurrentExperience.Value, _experienceHandle.CurrentLevel.Value, requiredExperienceForNextLevel);
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
            UpdateDisplay(_experienceHandle.CurrentExperience.Value, valueLevel, requiredExperienceForNextLevel);
        }


        private void OnExperienceChanged(float valueExperience)
        {
            if (valueExperience >= requiredExperienceForNextLevel)
            {
                MakeLevelUp();

                requiredExperienceForNextLevel = requiredExperienceForNextLevel * progressRate;
            }

            UpdateDisplay(valueExperience, _experienceHandle.CurrentLevel.Value, requiredExperienceForNextLevel);
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

