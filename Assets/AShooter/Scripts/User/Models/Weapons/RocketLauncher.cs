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

        public RocketLauncher(GameObject weaponObject, LayerMask layerMask, ParticleSystem effect, float damage, float effectDestroyDelay, Projectile projectileObject)
        {
            Debug.Log("INIT RocketLauncher");

            WeaponObject = weaponObject;
            LayerMask = layerMask;
            Effect = effect;
            Damage = damage;
            EffectDestroyDelay = effectDestroyDelay;
            WeaponType = WeaponType.RocketLauncher;
            ProjectileObject = projectileObject;
        }


        public override void Shoot(Camera camera, Vector3 mousePosition)
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

            PlayerProjectileAttack projectileAttack = new PlayerProjectileAttack(this, muzzle);
            projectileAttack.Attack();
        }


    }
}
