using UniRx;
using UnityEngine;


namespace User
{

    public sealed class Pistol : Weapon
    {

        public Pistol(int weaponId, GameObject weaponObject, Sprite weaponIcon, Projectile projectileObject, 
            WeaponType weaponType, float damage, int clipSize, ReactiveProperty<int> leftPatronsCount, 
            float reloadTime, float shootDistance, float shootSpeed, float fireSpread, LayerMask layerMask, 
            ParticleSystem muzzleEffect, ParticleSystem effect, float effectDestroyDelay, Camera camera) : base(
            weaponId, weaponObject, weaponIcon, projectileObject, weaponType, damage, clipSize, leftPatronsCount,
            reloadTime, shootDistance, shootSpeed, fireSpread, layerMask, muzzleEffect, effect, effectDestroyDelay, camera)
        {

        }


    }
}
