using Abstracts;
using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(WeaponConfig), menuName = "Config/" + nameof(WeaponConfig), order = 0)]
    public sealed class WeaponConfig : ScriptableObject, IWeapon
    {
        
        [field: SerializeField, Min(0f)] public float Damage { get; set; }
        [field: SerializeField] public GameObject WeaponPrefab { get; set; }
        [field: SerializeField] public LayerMask LayerMask { get; set; }

        [field: SerializeField] public ParticleSystem EffectPrefab { get; set; }
        [field: SerializeField, Min(0f)] public float EffectDestroyDelay { get; set; }


    }
}
