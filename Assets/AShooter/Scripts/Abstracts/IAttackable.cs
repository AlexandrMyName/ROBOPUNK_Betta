using System;
using UniRx;


namespace Abstracts
{
    
    public interface IAttackable 
    {
        
        ReactiveProperty<float> Health { get;  }
        ReactiveProperty<bool> IsDeadFlag { get; set; }
        void TakeDamage(float amountHealth);
        void SetMaxHealth(float maxHealth, Action<ReactiveProperty<float>> onCompleted = null);
        void SetAttackableDamage(float attackForceDamage);
    }
}