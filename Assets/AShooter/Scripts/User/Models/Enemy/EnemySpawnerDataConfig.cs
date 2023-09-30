using Core.DTO;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(EnemySpawnerDataConfig), menuName = "Config/" + nameof(EnemySpawnerDataConfig))]
    public class EnemySpawnerDataConfig : ScriptableObject
    {

        [Header("Endless waves: ")]
        public bool loop;


        [Space(10)]
        [Header("List waves: ")]
        public List<EnemyWaveConfig> Wave;


        [Space(10)]
        [Header("Temporary configs")]
        public float respawnWaveDelay;

    }


    [Serializable]
    public class EnemyWaveConfig
    {

        [Header("List of enemies in the wave: ")]
        public List<EnemyConfig> Enemy;


        [Space(10)]
        [Header("Temporary configs")]
        public float respawnEnemyInWaveDelay;


        [Header("Spawn distance configs: ")]
        [Space(10)]
        public float spawnRadiusRelativeToPlayer;


        [Space(10)]
        [Header("Number enemies in wave: ")]
        public int numberEnemiesInWave;
        public int numberEnemiesInScene;

    }


    [Serializable]
    public class EnemyConfig
    {

        [Header("Description enemy configs: ")]
        public string description;
        public string nameEnemy;
        public EnemyType enemyType;


        [Space(10)]
        [Header("Prefab configs: ")]
        public GameObject prefab;


        [Space(10)]
        [Header("Health configs: ")]
        public float maxHealth;


        [Space(10)]
        [Header("Attack configs: ")]
        public float maxAttackDamage;
        public float attackDistance;
        public float attackFrequency;


        [Space(10)]
        [Header("Gold configs: ")]
        [Range(0, 100)]
        public int goldDropRate;
        [SerializeField]
        public Range goldValueRange;


        [Space(10)]
        [Header("Experience configs: ")]
        public Range experienceRange;


        [Space(10)]
        [Range(0, 1)]
        [Header("Install with the calculation of the remaining weights. The sum of all weights of each enemy type in a wave MUST be equal to [1] !!!")]
        public float probabilityWeigh;


        [Serializable]
        public struct Range
        {
            public int min;
            public int max;
        }

    }
}