using UnityEngine;


namespace User
{

    public class Pistol : Weapon
    {

        public Pistol(GameObject weaponObject, LayerMask layerMask, ParticleSystem effect, float damage, float effectDestroyDelay)
        {
            Debug.Log("INIT PISTOL");

            WeaponObject = weaponObject;
            LayerMask = layerMask;
            Effect = effect;
            Damage = damage;
            EffectDestroyDelay = effectDestroyDelay;
            WeaponType = WeaponType.Pistol;
        }


    }
}
