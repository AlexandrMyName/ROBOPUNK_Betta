using System;
using UnityEngine;
using UnityEngine.Serialization;


namespace User.Player
{
    
    [CreateAssetMenu(fileName = nameof(PlayerMetaStatsMultiplier), menuName = "Config/" + nameof(PlayerMetaStatsMultiplier))]
    public sealed class PlayerMetaStatsMultiplier : ScriptableObject
    {
        [field: SerializeField] public PlayerMeta PlayerMeta { get; private set; }
    }
    
    
    [Serializable]
    public struct PlayerMeta
    {
        public float BaseHealthMultiplier;
        public float BaseDamageMultiplier;
        public float BaseMoveSpeedMultiplier;
        public float BaseShieldCapacityMultiplier;
        public float BaseDashDistanceMultiplier;
        public float BaseShootSpeedMultiplier;
    }
    
}