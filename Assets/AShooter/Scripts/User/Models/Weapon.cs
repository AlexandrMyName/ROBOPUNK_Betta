using Abstracts;
using UnityEngine;


namespace User
{

    public sealed class Weapon : IWeapon
    {

        public WeaponConfig _weaponConfig;

        public  float Damage { get; set; }
        public GameObject WeaponPrefab { get; set; }
        public LayerMask LayerMask { get; set; }
        public ParticleSystem EffectPrefab { get; set; }
        public float EffectDestroyDelay { get; set; }


    }
}
