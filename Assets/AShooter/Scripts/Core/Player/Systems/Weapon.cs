using UnityEngine;


namespace Core
{

    [CreateAssetMenu(fileName = nameof(Weapon), menuName = "Settings/" + nameof(Weapon), order = 0)]
    public sealed class Weapon : ScriptableObject
    {

        [field: SerializeField, Min(0f)] public float Damage { get; private set; }
        [field: SerializeField] public LayerMask layerMask;

        [Header("Effects")]
        [field: SerializeField] public ParticleSystem effectPrefab;
        [field: SerializeField, Min(0f)] public float effectDestroyDelay = 2f;


    }
}
