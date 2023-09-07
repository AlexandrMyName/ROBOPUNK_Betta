using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Core
{

    public class EnemyAttackSystem : BaseSystem
    {

        private List<IDisposable> _disposables = new();
        private Collider _enemyRadiusAttack;
        private Enemy _enemy;


        protected override void Awake(IGameComponents components)
        {
            _enemy = components.BaseObject.GetComponent<Enemy>();
            _enemyRadiusAttack = _enemy.EnemyRadiusAttack;

            _enemyRadiusAttack.OnTriggerStayAsObservable()
                .Where(col => col.GetComponent<Player>() != null)  //gameObject.CompareTag("Player"))
                .ThrottleFirst(TimeSpan.FromSeconds(GameLoopManager.EnemyAttackFrequency))
                .Subscribe(_hit => HandleTriggerCollider(_hit))
                .AddTo(_disposables);
        }


        protected override void OnEnable()
            => _enemy.SetAttackableDamage(GameLoopManager.EnemyDamageForce);


        private void HandleTriggerCollider(Collider collider)
        {
            var player = collider.GetComponent<IAttackable>();
            player.TakeDamage(_enemy.Damage);

#if UNITY_EDITOR
            Debug.Log($"PLAYER: {player.Health}  ( HIT -{_enemy.Damage}!)");
#endif
        }


    }

}