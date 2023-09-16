using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(ExplosionAbilityConfig), menuName = "Config/" + nameof(ExplosionAbilityConfig), order = 0)]
    public sealed class ExplosionAbilityConfig : ScriptableObject
    {

        [field: SerializeField] public Explosion ExplosionObject { get; private set; }

        [field: SerializeField, Min(0f)] public float Damage { get; private set; }

        [field: SerializeField, Min(0f)] public int Radius { get; private set; }

        [field: SerializeField, Min(0f)] public float Force { get; set; }

        [field: SerializeField, Min(0f)] public float Lifetime { get; private set; }

        [field: SerializeField, Min(0f)] public float UsageTimeout { get; private set; }

        [field: SerializeField] public LayerMask LayerMask { get; private set; }

        [field: SerializeField] public ParticleSystem Effect { get; private set; }

        [field: SerializeField, Min(0f)] public float EffectDestroyDelay { get; private set; }


    }
}
