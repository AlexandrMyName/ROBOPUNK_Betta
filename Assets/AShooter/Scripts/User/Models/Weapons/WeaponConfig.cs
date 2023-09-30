using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(WeaponConfig), menuName = "Config/" + nameof(WeaponConfig))]
    public sealed class WeaponConfig : ScriptableObject
    {

        [field: SerializeField] public int WeaponId { get; private set; }

        [field: SerializeField] public GameObject WeaponObject { get; private set; }

        [field: SerializeField] public Sprite WeaponIcon { get; private set; }

        [field: SerializeField] public Projectile ProjectileObject { get; private set; }

        [field: SerializeField] public float ProjectileForce { get; private set; }

        [field: SerializeField] public WeaponType WeaponType { get; private set; }

        [field: SerializeField, Min(0f)] public float Damage { get; private set; }

        [field: SerializeField, Min(0f)] public int ClipSize { get; private set; }

        [field: SerializeField, Min(0f)] public int LeftPatronsCount { get; private set; }

        [field: SerializeField, Min(0f)] public float ReloadTime { get; private set; }

        [field: SerializeField, Min(0f)] public float ShootDistance { get; private set; }

        [field: SerializeField, Min(0f)] public float ShootSpeed { get; private set; }

        [field: SerializeField, Min(1f)] public float FireSpread { get; private set; }

        [field: SerializeField] public float SpreadFactor { get; private set; }

        [field: SerializeField] public LayerMask LayerMask { get; private set; }

        [field: SerializeField] public ParticleSystem MuzzleEffect { get; private set; }

        [field: SerializeField] public ParticleSystem Effect { get; private set; }

        [field: SerializeField, Min(0f)] public float EffectDestroyDelay { get; private set; }


    }
}
