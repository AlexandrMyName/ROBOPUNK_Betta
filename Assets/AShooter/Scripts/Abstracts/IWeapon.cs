using UnityEngine;


namespace Abstracts
{

    internal interface IWeapon
    {

        float Damage { get; set; }
        GameObject WeaponPrefab { get; set; }
        LayerMask LayerMask { get; set; }
        ParticleSystem EffectPrefab { get; set; }
        float EffectDestroyDelay { get; set; }


    }
}
