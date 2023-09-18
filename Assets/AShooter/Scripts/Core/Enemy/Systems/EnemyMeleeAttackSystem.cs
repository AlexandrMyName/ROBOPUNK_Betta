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
        private IEnemy _enemy;

        protected override void Awake(IGameComponents components)
        {
            _enemy = components.BaseObject.GetComponent<Enemy>();

            var enemyRadiusAttack = _enemy.EnemyRadiusAttack;

            enemyRadiusAttack.OnTriggerStayAsObservable()
                .Where(col => col.GetComponent<IPlayer>() != null)
                .ThrottleFirst(TimeSpan.FromSeconds(GameLoopManager.EnemyAttackFrequency))
                .Subscribe(_hit => HandleTriggerCollider(_hit))
                .AddTo(_disposables);
        }


        protected override void OnEnable()
            => _enemy.ComponentsStore.Attackable.SetAttackableDamage(GameLoopManager.EnemyDamageForce);


        private void HandleTriggerCollider(Collider collider)
        {
            var playerAttackableComponent = collider.GetComponent<IPlayer>().ComponentsStore.Attackable;
            playerAttackableComponent.TakeDamage(_enemy.ComponentsStore.Attackable.Damage);

            #if UNITY_EDITOR
            Debug.Log($"PLAYER: {playerAttackableComponent.Health}  ( HIT -{_enemy.ComponentsStore.Attackable.Damage}!)");
            #endif
        }


    }

}