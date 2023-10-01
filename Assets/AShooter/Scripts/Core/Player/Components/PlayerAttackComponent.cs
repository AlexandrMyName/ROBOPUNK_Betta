using Abstracts;
using UniRx;
using System;
using Zenject;
using System.Diagnostics;

namespace Core
{


    public class PlayerAttackComponent : IAttackable
    {

        public PlayerAttackComponent()
        {

            HealthProtection = new ReactiveProperty<float>();
            IsIgnoreDamage = false;
        }

        [Inject(Id = "PlayerHealth")] public ReactiveProperty<float> Health { get; }

        public ReactiveProperty<bool> IsDeadFlag { get; set; }

        public ReactiveProperty<float> HealthProtection { get; private set; }

        public bool IsIgnoreDamage { get; set; }


        public void SetAttackableDamage(float attackForceDamage) { }


        public void SetMaxHealth(float maxHealth, Action<ReactiveProperty<float>> onCompleted = null) { }


        public void TakeDamage(float amountDamage)
        {
            UnityEngine.Debug.Log(IsIgnoreDamage);
            if (IsIgnoreDamage) return;
            
            else
            {
                TakeProtectionDamage(amountDamage);
            }
        }


        private void TakeProtectionDamage(float amounDamage)
        {
            UnityEngine.Debug.LogWarning("Shield ");
            HealthProtection.Value -= amounDamage;

            if(HealthProtection.Value < 0)
            {
                UnityEngine.Debug.LogWarning("Shield bricked");
                Health.Value -= Math.Abs(HealthProtection.Value);
            }
        }
        
    }
}