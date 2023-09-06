using Abstracts;
using Core;
using UnityEngine;


namespace User
{
    public class RocketLauncher : Weapon
    {

        public RocketLauncher(GameObject weaponObject, LayerMask layerMask, ParticleSystem effect, float damage, float effectDestroyDelay)
        {
            Debug.Log("INIT RocketLauncher");

            WeaponObject = weaponObject;
            LayerMask = layerMask;
            Effect = effect;
            Damage = damage;
            EffectDestroyDelay = effectDestroyDelay;
            WeaponType = WeaponType.RocketLauncher;
        }


        public override void Shoot(Camera camera, Vector3 mousePosition)
        {
            PlayerProjectileAttack projectileAttack = new PlayerProjectileAttack(this, WeaponObject.transform);
            projectileAttack.Attack();
        }


    }
}
