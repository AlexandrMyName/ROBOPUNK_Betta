using UniRx;
using UnityEngine;


namespace Abstracts
{

    public interface IExperienceHandle
    {

        public ReactiveProperty<float> CurrentExperience { get; }

        public ReactiveProperty<int> CurrentLevel { get; }

        public ParticleSystem ExperienceBall { get; }

        public ParticleSystem LevelUpEffect { get; }

        public void AddExperience(float amountExp);

    }
}
