using UnityEngine;


namespace Abstracts
{

    internal interface IWeapon
    {

        GameObject WeaponObject { get; }
        
        LayerMask LayerMask { get; }
        
        ParticleSystem Effect { get; }
        
        float Damage { get; }
        
        float EffectDestroyDelay { get; }


    }
}
