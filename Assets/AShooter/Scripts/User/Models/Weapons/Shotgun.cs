using UnityEngine;


namespace User
{
    public class Shotgun : Weapon
    {
               
        public Shotgun(int weaponId, GameObject weaponObject, Projectile projectileObject, WeaponType weaponType, float damage, int clipSize, int leftPatronsCount,
            float reloadTime, float shootDistance, float shootSpeed, float fireSpread, LayerMask layerMask, ParticleSystem effect, float effectDestroyDelay)
        {
            WeaponId = weaponId;
            WeaponObject = weaponObject;
            ProjectileObject = projectileObject;
            WeaponType = weaponType;
            Damage = damage;
            ClipSize = clipSize;
            LeftPatronsCount = leftPatronsCount;
            ReloadTime = reloadTime;
            ShootDistance = shootDistance;
            ShootSpeed = shootSpeed;
            FireSpread = fireSpread;
            LayerMask = layerMask;
            Effect = effect;
            EffectDestroyDelay = effectDestroyDelay;
        }


    }
}
