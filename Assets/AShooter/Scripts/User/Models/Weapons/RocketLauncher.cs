using Abstracts;
using Core;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;


namespace User
{
    public class RocketLauncher : Weapon
    {

        public RocketLauncher(int weaponId, GameObject weaponObject, Projectile projectileObject, WeaponType weaponType, float damage, int clipSize, int leftPatronsCount,
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


        public override void Shoot(Transform playerTransform, Camera camera, Vector3 mousePosition)
        {
            Transform muzzle = null;
            foreach (Transform child in WeaponObject.transform)
            {
                if (child.CompareTag("Muzzle"))
                {
                    muzzle = child;
                    break;
                }
            }

            PlayerProjectileAttack projectileAttack = new PlayerProjectileAttack(this, muzzle, camera, mousePosition);
            projectileAttack.Attack();
        }


    }
}
