using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;


namespace Core
{

    public class PlayerExperienceSystem : BaseSystem , IDisposable
    {
        
        private List<IDisposable> _disposables;
        private IExperienceView _experienceView;


        protected override void Awake(IGameComponents components)
        {
            _disposables = new();

            _experienceView = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.ExperienceView;

            var currentExperience = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.ExperienceHandle.CurrentExperience;

            currentExperience.Subscribe(UpdateDisplay);

            _experienceView.Show();

            _experienceView.ChangeDisplay(currentExperience.Value);
        }


        private void UpdateDisplay(float value)
        {
            _experienceView.ChangeDisplay(value);
        }


        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());


    }
}

