using Abstracts;
using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(WeaponConfig), menuName = "Config/" + nameof(WeaponConfig), order = 0)]
    public sealed class WeaponConfig : ScriptableObject
    {
        
        [field: SerializeField] public GameObject WeaponPrefab { get; private set; }
        
        [field: SerializeField] public ParticleSystem EffectPrefab { get; private set; }
        
        [field: SerializeField, Min(0f)] public float Damage { get; private set; }
        
        [field: SerializeField] public LayerMask LayerMask { get; private set; }
        
        [field: SerializeField, Min(0f)] public float EffectDestroyDelay { get; private set; }

        [field: SerializeField] public WeaponType WeaponType { get; private set; }

        [field: SerializeField] public int WeapoId { get; private set; }


    }
}
