using Core.DTO;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(EnemyDataConfig), menuName = "Config/" + nameof(EnemyDataConfig))]
    public class EnemyDataConfig : ScriptableObject
    {
        public List<EnemyConfig> Configs;
    }
    
    //[Serializable]
    //public class EnemyConfig
    //{
    //
    //    [Header("Description enemy configs: ")]
    //    public string description;
    //    public string nameEnemy;
    //    public EnemyType enemyType;
    //
    //
    //    [Space(10)]
    //    [Header("Prefab configs: ")]
    //    public GameObject prefab;
    //
    //
    //    [Space(10)]
    //    [Header("Health configs: ")]
    //    public float maxHealth;
    //
    //
    //    [Space(10)]
    //    [Header("Attack configs: ")]
    //    public float maxAttackDamage;
    //    public float attackDistance;
    //    public float attackFrequency;
    //
    //
    //    [Space(10)]
    //    [Header("Gold configs: ")]
    //    [Range(0, 100)]
    //    public float goldDropRate;
    //    [SerializeField]
    //    public Range goldValueRange;
    //
    //
    //    [Space(10)]
    //    [Header("Experience configs: ")]
    //    public Range experienceRange;
    //
    //
    //    [Space(10)]
    //    [Header("Temporary configs")]
    //    public float respawnDelay;
    //
    //
    //    [Serializable]
    //    public struct Range
    //    {
    //        public int min;
    //        public int max;
    //    }
    //
    //
    //}
}