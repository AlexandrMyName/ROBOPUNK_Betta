using UniRx;
using UnityEngine;

namespace Abstracts
{

    public interface IEnemyAttackable : IAttackable
    {
        ReactiveProperty<bool> IsRewardReadyFlag { get; set; }

        float Damage { get; set; }

        float AttackDistance { get; set; }

        float AttackFrequency { get; set; }

        ReactiveProperty<bool> IsCameAttackPosition { get; }

        void TakeDamage(float amountHealth, RaycastHit hit, Vector3 direction);

        RaycastHit CachedHitDamage { get; set; }
        Vector3 CachedDirectionDamage { get; set; }
    }

}