using UniRx;


namespace Abstracts
{

    public interface IEnemyAttackable : IAttackable
    {

        float Damage { get; set; }

        float AttackDistance { get; set; }

        float AttackFrequency { get; set; }

        ReactiveProperty<bool> IsCameAttackPosition { get; }

    }

}