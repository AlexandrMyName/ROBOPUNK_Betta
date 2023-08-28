using UniRx;

namespace abstracts
{
    public interface IAttackable 
    {
        ReactiveProperty<float> Health { get;  }
        void TakeDamage(float amountHealth);
    }
}