using UnityEngine;


namespace User
{
    public class Shotgun : Weapon
    {
               
        public Shotgun(GameObject weaponObject, LayerMask layerMask, ParticleSystem effect, float damage, float effectDestroyDelay)
        {
            Debug.Log("INIT Shotgun");

            WeaponObject = weaponObject;
            LayerMask = layerMask;
            Effect = effect;
            Damage = damage;
            EffectDestroyDelay = effectDestroyDelay;
            WeaponType = WeaponType.Shotgun;
        }


    }
}
