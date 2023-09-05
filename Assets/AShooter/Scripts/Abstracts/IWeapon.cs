using UnityEngine;
using User;


namespace Abstracts
{

    public interface IWeapon
    {

        GameObject WeaponObject { get; }
        
        LayerMask LayerMask { get; }
        
        ParticleSystem Effect { get; }

        WeaponType WeaponType { get; }

        int WeaponId { get; }

        float Damage { get; }
        
        float EffectDestroyDelay { get; }

        int ClipSize { get; }

        int LeftPatronsCount { get; }

        float ReloadTime { get; }

        float ShootDistance { get; }

        float ShootSpeed { get; }

        float FireSpread { get; }


        void Shoot();


    }
}
