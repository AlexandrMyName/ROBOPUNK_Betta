using Abstracts;
using UniRx;
#if UNITY_EDITOR
using UnityEngine;
using User;
#endif


namespace Core
{


    public class PlayerExperienceComponent : IExperienceHandle
    {

        public ReactiveProperty<float> CurrentExperience { get; }

        public ReactiveProperty<int> CurrentLevel { get; }

        public ParticleSystem ExperienceBall { get; }


        public PlayerExperienceComponent(ExperienceConfig experienceConfig)
        {
            CurrentExperience = new ReactiveProperty<float>(0);
            CurrentLevel = new ReactiveProperty<int>(1);
            ExperienceBall = experienceConfig.ExperienceBall;
        }


        public void AddExperience(float amountExp)
        {
            CurrentExperience.Value += amountExp;
        }


    }
}