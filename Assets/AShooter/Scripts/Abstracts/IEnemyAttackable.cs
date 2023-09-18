using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


namespace Abstracts
{
    public interface IEnemyAttackable : IAttackable
    {
         float Damage { get; set; }


         ReactiveProperty<float> RangedAttackRange { get;  }
         ReactiveProperty<bool> IsCameAttackPosition { get; }


        void InitComponent(float rangedAttackRange);
    }
}