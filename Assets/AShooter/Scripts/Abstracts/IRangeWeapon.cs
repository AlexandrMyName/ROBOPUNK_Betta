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

        int LeftPatronsCount { get; }

        float ReloadTime { get; }

        float ShootDistance { get; }

        float ShootSpeed { get; }

        float FireSpread { get; }

        float SpreadFactor { get; }

        
        void Shoot(Transform playerTransform, Camera camera, Vector3 mousePosition);

        void Reload();

        void ProcessReload();
        
        bool IsReloadProcessing { get; }

        bool IsShootReady { get; }
        
        
    }
}