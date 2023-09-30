using UniRx;
using UnityEngine;
using User;


namespace Abstracts
{

    public interface IAbility
    {

        Sprite ExplosionIcon { get; }
        AbilityType AbilityType { get; }
        float UsageTimeout { get; }
        ReactiveProperty<bool> IsReady { get; }


    }
}
