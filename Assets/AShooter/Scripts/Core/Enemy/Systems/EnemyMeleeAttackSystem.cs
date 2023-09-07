using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Core
{

    public class EnemyMeleeAttackSystem : BaseSystem
    {

        private List<IDisposable> _disposables = new();
        private Enemy _enemy;
        private ReactiveProperty<bool> _isReadyToMeleeAttack;

        protected override void Awake(IGameComponents components)
        {
            _enemy = components.BaseObject.GetComponent<Enemy>();

            var enemyRadiusAttack = _enemy.EnemyRadiusAttack;

            enemyRadiusAttack.radius = GameLoopManager.EnemyMeleeAttackRange;

            enemyRadiusAttack.OnTriggerStayAsObservable()
                .Where(col => col.GetComponent<Player>() != null)
                .ThrottleFirst(TimeSpan.FromSeconds(GameLoopManager.EnemyAttackFrequency))
                .Subscribe(_hit => HandleTriggerCollider(_hit))
                .AddTo(_disposables);

            _isReadyToMeleeAttack = _enemy.IsReadyToMeleeAttack;

            enemyRadiusAttack.OnTriggerExitAsObservable()
            .Where(col => col.GetComponent<Player>() != null)
            .Subscribe(_ => _isReadyToMeleeAttack.Value = false)
            .AddTo(_enemy);
        }


        protected override void OnEnable()
            => _enemy.SetAttackableDamage(GameLoopManager.EnemyDamageForce);


        private void HandleTriggerCollider(Collider collider)
        {
            var player = collider.GetComponent<IAttackable>();
            player.TakeDamage(_enemy.Damage);

            _isReadyToMeleeAttack.Value = true;

            #if UNITY_EDITOR
            Debug.Log($"PLAYER: {player.Health}  ( HIT -{_enemy.Damage}!)");
            #endif
        }


    }

}