using Abstracts;
using UniRx;
#if UNITY_EDITOR
using UnityEngine;
#endif


namespace Core
{


    public class PlayerExpComponent : IExperienceAccumulation
    {

        public ReactiveProperty<float> CurrentExp { get; }

        public PlayerExpComponent()
        {
            CurrentExp = new ReactiveProperty<float>(0);
        }


        public void AddExp(float amountExp)
        {
            CurrentExp.Value += amountExp;
#if UNITY_EDITOR
            Debug.Log($"Add Exp -> {amountExp}, Exp Account -> {CurrentExp.Value}");
#endif
        }


    }
}