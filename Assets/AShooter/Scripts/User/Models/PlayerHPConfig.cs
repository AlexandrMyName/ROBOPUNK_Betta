using UnityEngine;

namespace User
{
    [CreateAssetMenu(fileName = nameof(PlayerHPConfig), menuName = "Config/" + nameof(PlayerHPConfig), order = 0)]
    public class PlayerHPConfig : ScriptableObject
    {
        [Range(100f, 400f)] public float DeathPunchForce = 200f;

    }
}