using Core;
using UniRx;
using UnityEngine;


namespace User
{
    
    public sealed class RocketLauncher : Weapon
    {

        public RocketLauncher(int weaponId, GameObject weaponObject, Sprite weaponIcon, Projectile projectileObject, 
            float projectileForce, WeaponType weaponType, float damage, int clipSize, 
            ReactiveProperty<int> leftPatronsCount, float reloadTime, float shootDistance, float shootSpeed,
            float fireSpread, LayerMask layerMask, ParticleSystem muzzleEffect, ParticleSystem effect,
            float effectDestroyDelay, Camera camera) : base(
            weaponId, weaponObject, weaponIcon, projectileObject, weaponType, damage, clipSize, leftPatronsCount,
            reloadTime, shootDistance, shootSpeed, fireSpread, layerMask, muzzleEffect, effect, effectDestroyDelay, 
            camera)
        {
            ProjectileForce = projectileForce;
        }
        

    }
}
