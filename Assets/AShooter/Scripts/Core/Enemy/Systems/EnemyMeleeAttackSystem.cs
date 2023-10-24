using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.AI;


namespace Core
{

    public class EnemyMeleeAttackSystem : BaseSystem, IDisposable
    {

        private List<IDisposable> _disposables = new();
        private IEnemy _enemy;
        private float _attackDistance;
        private float _attackFrequency;
        private NavMeshAgent _navMeshAgent;
        private IGameComponents _components;

        public void Dispose()
        {
            _disposables.ForEach(disp => disp.Dispose());
        }

        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _enemy = components.BaseObject.GetComponent<Enemy>();
            _navMeshAgent = components.BaseObject.GetComponent<NavMeshAgent>();
            _attackDistance = _enemy.ComponentsStore.Attackable.AttackDistance;
            _attackFrequency = _enemy.ComponentsStore.Attackable.AttackFrequency;

            var sphereCollider = components.BaseObject.AddComponent<SphereCollider>();
            sphereCollider.radius = _attackDistance;
            sphereCollider.isTrigger = true;
            
            sphereCollider.OnTriggerStayAsObservable()
                .Where(col => col.GetComponent<IPlayer>() != null)
                .ThrottleFirst(TimeSpan.FromSeconds(_attackFrequency))
                .Subscribe(_hit => HandleTriggerCollider(_hit))
                .AddTo(_disposables);
        }


        private void HandleTriggerCollider(Collider collider)
        {

            if (!_components.BaseObject.activeSelf
               ||
               !_navMeshAgent.isActiveAndEnabled) return;
            var playerAttackableComponent = collider.GetComponent<IPlayer>().ComponentsStore.Attackable;
            playerAttackableComponent.TakeDamage(_enemy.ComponentsStore.Attackable.Damage);

        }


    }
}