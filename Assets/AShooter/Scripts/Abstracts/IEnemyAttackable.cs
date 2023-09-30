using UniRx;


namespace Abstracts
{

    public interface IEnemyAttackable : IAttackable
    {
        ReactiveProperty<bool> IsRewardReadyFlag { get; set; }

        float Damage { get; set; }

        float AttackDistance { get; set; }

        float AttackFrequency { get; set; }

        ReactiveProperty<bool> IsCameAttackPosition { get; }

    }

}