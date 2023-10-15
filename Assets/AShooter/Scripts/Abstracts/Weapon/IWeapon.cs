using System;
using UnityEngine;
using User;


namespace Abstracts
{

    public interface IWeapon : IDisposable
    {
        
        int WeaponId { get; }

        GameObject WeaponObject { get; }

        Sprite WeaponIcon { get; }

        WeaponType WeaponType { get; }

        float Damage { get; set; }
        
        LayerMask LayerMask { get; }

        ParticleSystem Effect { get; }

        float EffectDestroyDelay { get; }
        
        
    }
}
