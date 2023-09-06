using UnityEngine;
using User;


namespace Abstracts
{

    public interface IWeapon
    {

        int WeaponId { get; }

        GameObject WeaponObject { get; }

        WeaponType WeaponType { get; }

        float Damage { get; }

        int ClipSize { get; }

        int LeftPatronsCount { get; }

        float ReloadTime { get; }

        float ShootDistance { get; }

        float ShootSpeed { get; }

        float FireSpread { get; }

        LayerMask LayerMask { get; }

        ParticleSystem Effect { get; }

        float EffectDestroyDelay { get; }


        void Shoot(Camera camera, Vector3 mousePosition);


    }
}
