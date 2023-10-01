using Abstracts;
using UniRx;
using System;
using Zenject;
using System.Diagnostics;

namespace Core
{


    public class PlayerAttackComponent : IAttackable
    {
        [Inject(Id = "PlayerHealth")] public ReactiveProperty<float> Health { get; }

        public ReactiveProperty<bool> IsDeadFlag { get; set; }
        public bool IsIgnoreDamage { get; set; }

        public void SetAttackableDamage(float attackForceDamage) { }

        public void SetMaxHealth(float maxHealth, Action<ReactiveProperty<float>> onCompleted = null) { }

        public void TakeDamage(float amountDamage)
        {

            if (IsIgnoreDamage) return;
            UnityEngine.Debug.LogWarning("Player Damage");
                Health.Value -= amountDamage;
        }
    }
}