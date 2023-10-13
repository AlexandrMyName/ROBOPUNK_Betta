using Abstracts;
using Core;
using Core.DTO;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using User;
using Zenject;


public class BosSpawner : MonoBehaviour
{
  
    [SerializeField] private EnemyConfig _config;
    [SerializeField] private GameObject _enemyViews_Prefab;
    [Inject(Id = "PlayerComponents")] private IComponentsStore _componentsPlayer;
    [Inject] DiContainer _container;


    private void Start()
    {

        SpawnBoss();
    }


    private void SpawnBoss()
    {

        var bossInstance = _container.InstantiatePrefab(_config.prefab);
        IEnemy enemy = bossInstance.GetComponent<IEnemy>();

        enemy.SetComponents(

            new EnemyComponentsStore(

                new EnemyAttackComponent(
                    _config.maxHealth,
                    _config.maxProtection,
                    _config.maxAttackDamage,
                    _config.attackDistance,
                    _config.attackFrequency),

                new EnemyPriceComponent(
                    _config.goldDropRate,
                    _config.goldValueRange,
                    _config.experienceRange)));

        enemy.SetSystems( CreateSystems(_config) );

        ViewsCreation(bossInstance.transform);
        bossInstance.SetActive(true);


       
    }

     
    private void ViewsCreation(Transform parent)
    {

        var viewsInstance = GameObject.Instantiate(_enemyViews_Prefab, parent, false);
   
        viewsInstance.transform.localPosition = Vector3.zero + Vector3.up * 1.5f;
        viewsInstance.GetComponent<IEnemyViews>().InitViews();
    }


    private List<ISystem> CreateSystems(EnemyConfig item)
    {

        Debug.LogWarning(_componentsPlayer == null);
        var systems = new List<ISystem>();

        systems.Add(new EnemyRewardSystem(_componentsPlayer.ExperienceHandle, _componentsPlayer.GoldWallet));
        systems.Add(new EnemyDamageSystem(item.maxHealth, item.maxProtection));
        systems.Add(new EnemyMovementSystem(_componentsPlayer.Movable.Rigidbody.transform));

        switch (item.enemyType)
        {
            case EnemyType.MeleeEnemy:
                systems.Add(new EnemyBossMeleeAttackSystem());
                break;

            case EnemyType.DistantEnemy:
                systems.Add(new EnemyDistantAttackSystem(_componentsPlayer.Movable.Rigidbody.transform));
                break;

            default:
                systems.Add(new EnemyBossMeleeAttackSystem());
                break;
        }
        return systems;
    }
}
