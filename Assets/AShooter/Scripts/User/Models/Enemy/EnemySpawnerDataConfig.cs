using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(EnemySpawnerDataConfig), menuName = "Config/" + nameof(EnemySpawnerDataConfig))]
    public class EnemySpawnerDataConfig : ScriptableObject
    {

        [Header("Spawn distance configs: ")]
        public float spawnRadiusRelativeToPlayer;


        [Space(10)]
        [Header("Waves configs")]
        public int numberWavesEnemy;
        public bool infiniteWavesEnemies;


        [Space(10)]
        [Header("Temporary configs")]
        public float respawnWavesDelay;


    }
}