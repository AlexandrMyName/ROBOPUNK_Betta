using Abstracts;
using UniRx;
#if UNITY_EDITOR
using UnityEngine;
#endif


namespace Core
{


    public class PlayerExperienceComponent : IExperienceHandle
    {

        public ReactiveProperty<float> CurrentExperience { get; }

        public ReactiveProperty<int> CurrentLevel { get; }

        public PlayerExperienceComponent()
        {
            CurrentExperience = new ReactiveProperty<float>(0);
            CurrentLevel = new ReactiveProperty<int>(1);
        }


        public void AddExperience(float amountExp)
        {
            CurrentExperience.Value += amountExp;
        }


    }
}