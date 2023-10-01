using System;
using UniRx;


namespace Abstracts
{
    
    public interface IAttackable 
    {

        bool IsIgnoreDamage { get; set; }
        ReactiveProperty<float> Health { get;  }
        ReactiveProperty<float> HealthProtection { get; }
        ReactiveProperty<bool> IsDeadFlag { get; set; }
        void TakeDamage(float amountHealth);
        void SetMaxHealth(float maxHealth, Action<ReactiveProperty<float>> onCompleted = null);
        void SetAttackableDamage(float attackForceDamage);

    }
}