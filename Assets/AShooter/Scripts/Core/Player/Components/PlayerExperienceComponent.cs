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

        public PlayerExperienceComponent()
        {
            CurrentExperience = new ReactiveProperty<float>(0);
        }


        public void AddExperience(float amountExp)
        {
            CurrentExperience.Value += amountExp;
        }


    }
}