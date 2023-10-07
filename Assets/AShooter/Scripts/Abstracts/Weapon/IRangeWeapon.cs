using UniRx;
using UnityEngine;
using User;


namespace Abstracts
{
    
    public interface IRangeWeapon : IWeapon
    {

        Projectile ProjectileObject { get; }

        Laser Laser { get; }

        float ProjectileForce { get; }

        int ClipSize { get; }

        ReactiveProperty<int> LeftPatronsCount { get; }

        float ReloadTime { get; }

        float ShootDistance { get; }

        float ShootSpeed { get; }

        float FireSpread { get; }

        float SpreadFactor { get; }

        ParticleSystem MuzzleEffect { get; }

        float MuzzleEffectDestroyDelay { get; }

        GameObject FakeWeaponObject { get;  }

        GameObject FakeBulletsObject { get; }


        void Shoot(Vector3 mousePosition);

        void Reload();

        void ProcessReload();

        ReactiveProperty<bool> IsReloadProcessing { get; }

        bool IsShootReady { get; }
        
        
    }
}