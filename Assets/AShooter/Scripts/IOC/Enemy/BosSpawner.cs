using Abstracts;
using Core;
using Core.DTO;
using Random = UnityEngine.Random;
using DI.Spawn;
using System.Collections.Generic;
using UnityEngine;
using User;
using Zenject;
using System;
using UniRx;


public class BosSpawner : MonoBehaviour
{

    [SerializeField] private bool _canAttackForOtherEnemies;
 
    [SerializeField] private EnemyBossComponent _enemyBossComponent;
    [SerializeField] private EnemyConfig _config;
    [SerializeField] private GameObject _enemyViews_Prefab;
    private static EnemySpawnerController _enemiesSpawner;
    [Inject(Id = "PlayerComponents")] private IComponentsStore _componentsPlayer;
    [Inject] private DiContainer _container;

    private Action _onBossDeath;


    private void Awake() => Boss.BossSpawner = this;
     

    public static void Init(EnemySpawnerController spawnerOfEnemies)
    => _enemiesSpawner = spawnerOfEnemies;
        

    public void Spawn(Action onBossDeath)
    {

        Boss.IsBossDead = false;
        _onBossDeath = onBossDeath;
        _enemiesSpawner.StopSpawnProcess();
        var bossInstance = _container.InstantiatePrefab(_config.prefab);

        SetRandomPosition(bossInstance);
        IEnemy enemy = bossInstance.GetComponent<IEnemy>();

        var attackable = new EnemyAttackComponent(
                    _config.maxHealth,
                    _config.maxProtection,
                    _config.maxAttackDamage,
                    _config.attackDistance,
                    _config.attackFrequency);

        attackable
            .Health
                .Subscribe( val => {if(val <= 0 ) _onBossDeath?.Invoke(); });

        var price = new EnemyPriceComponent(
                    _config.goldDropRate,
                    _config.goldValueRange,
                    _config.experienceRange);

        enemy.SetComponents(

            new EnemyComponentsStore( attackable, price));

        enemy.SetSystems(CreateSystems(_config));

        ViewsCreation(bossInstance.transform);
        bossInstance.SetActive(true);

        

    }


    private void SetRandomPosition(GameObject bossInstance)
    {

        bossInstance.transform.position =
            new Vector3(
               Random.Range(
                   _componentsPlayer.Movable.Rigidbody.transform.position.x - 25,
                   _componentsPlayer.Movable.Rigidbody.transform.position.x + 25
               ),
               2f
                ,
               Random.Range(
                   _componentsPlayer.Movable.Rigidbody.transform.position.z - 25,
                   _componentsPlayer.Movable.Rigidbody.transform.position.z + 25
               ));
    }


    private void ViewsCreation(Transform parent)
    {

        var viewsInstance = GameObject.Instantiate(_enemyViews_Prefab, parent, false);
   
        viewsInstance.transform.localPosition = Vector3.zero + Vector3.up * 2f;
        viewsInstance.GetComponent<IEnemyViews>().InitViews();
    }


    private List<ISystem> CreateSystems(EnemyConfig item)
    {
         
        var systems = new List<ISystem>();

        systems.Add(new EnemyRewardSystem(_componentsPlayer.ExperienceHandle, _componentsPlayer.GoldWallet));
        systems.Add(new EnemyDamageSystem(item.maxHealth, item.maxProtection));
        systems.Add(new EnemyMovementSystem(_componentsPlayer.Movable.Rigidbody.transform));
         

        switch (item.enemyType)
        {
            case EnemyType.MeleeEnemy:
                 systems.Add(new EnemyBossMeleeAttackSystem(_canAttackForOtherEnemies));
                break;

            case EnemyType.DistantEnemy:
                 systems.Add(new EnemyBossDistantAttackSystem(_enemyBossComponent, _componentsPlayer.Movable.Rigidbody.transform));
                break;

                case EnemyType.DistantMeleeEnemy:

                 systems.Add(new EnemyBossMeleeAttackSystem(_canAttackForOtherEnemies));
                 systems.Add(new EnemyDistantAttackSystem(_componentsPlayer.Movable.Rigidbody.transform));
                break;
            default:
                 systems.Add(new EnemyBossMeleeAttackSystem(_canAttackForOtherEnemies));
                break;
        }
        return systems;
    }
}
 
public static class Boss { 
    
    public static BosSpawner BossSpawner { get; set; }
    public static bool IsBossDead { get; set; }

}