using UniRx;


namespace Abstracts
{
    
    public interface IAttackable 
    {
        
        ReactiveProperty<float> Health { get;  }
        
        void TakeDamage(float amountHealth);
        
        
    }
}