using Abstracts;
using UnityEngine;


namespace User
{

    public sealed class Weapon : IWeapon
    {
        public GameObject WeaponObject { get; }
        
        public LayerMask LayerMask { get; }
        
        public ParticleSystem Effect { get; }

        public  float Damage { get; }
        
        public float EffectDestroyDelay { get; }


        public Weapon(GameObject weaponObject, LayerMask layerMask, ParticleSystem effect, float damage, float effectDestroyDelay)
        {
            WeaponObject = weaponObject;
            LayerMask = layerMask;
            Effect = effect;
            Damage = damage;
            EffectDestroyDelay = effectDestroyDelay;
        }
        

    }
}
