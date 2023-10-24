using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.AI;


namespace Core
{

    public class EnemyBossMeleeAttackSystem : BaseSystem, IDisposable
    {

        private List<IDisposable> _disposables = new();
        private IEnemy _enemy;
        private float _attackDistance;
        private float _attackFrequency;
        private NavMeshAgent _navMeshAgent;
        private IGameComponents _components;
        private bool _canAttackOtherEnemies;

        public EnemyBossMeleeAttackSystem(bool isAttackOtherEnemies)
        => _canAttackOtherEnemies = isAttackOtherEnemies;
       

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
               .ThrottleFirst(TimeSpan.FromSeconds(_attackFrequency))
               .Subscribe(_hit => HandleTriggerCollider(_hit))
               .AddTo(_disposables);
             
        }

         

        private void HandleTriggerCollider(Collider collider )
        {

            bool isPlayer = collider.GetComponent<IPlayer>() != null;
            bool isEnemy = collider.GetComponent<IEnemy>() != null;


            if (!_components.BaseObject.activeSelf
                 || !_navMeshAgent.isActiveAndEnabled) return;


            if (isPlayer)
            {

                var playerAttackableComponent = collider.GetComponent<IPlayer>().ComponentsStore.Attackable;

                playerAttackableComponent.TakeDamage(_enemy.ComponentsStore.Attackable.Damage);
            }

            if (_canAttackOtherEnemies)
                AttackForEnemies(collider, isEnemy);

        }

        private void AttackForEnemies(Collider collider, bool isEnemy)
        {

            if (isEnemy)
            {

                RaycastHit hit = new();

                var enemyAttackableComponent = collider.GetComponent<IEnemy>().ComponentsStore.Attackable;

                enemyAttackableComponent.TakeDamage(2000, hit, Vector3.zero);
            }
            else
            {

                var mainObject = collider.GetComponentInParent<IEnemy>();

                if (mainObject != null)
                {
                    if (mainObject != _enemy)
                        mainObject.ComponentsStore.Attackable.TakeDamage(2000);
                }
            }
        }

        public void Dispose()
        {
            _disposables.ForEach(disp => disp.Dispose());
        }
    }
}