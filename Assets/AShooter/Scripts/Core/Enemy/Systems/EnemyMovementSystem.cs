using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Abstracts;
using System;
using UniRx;


namespace Core
{

    public class EnemyMovementSystem : BaseSystem
    {

        private List<IDisposable> _disposables = new();
        private NavMeshAgent _navMeshAgent;
        private Transform _targetPosition;
        private float _indentFromTarget;
        private IEnemy _enemy;
        private ReactiveProperty<bool> _isCameAttackPosition;


        public EnemyMovementSystem(Transform targetPosition)
        {
            _targetPosition = targetPosition;
        }


        protected override void Awake(IGameComponents components)
        {
            _navMeshAgent = components.BaseObject.GetComponent<NavMeshAgent>();
            _enemy = components.BaseObject.GetComponent<IEnemy>();
            _isCameAttackPosition = _enemy.ComponentsStore.Attackable.IsCameAttackPosition;
            _indentFromTarget = _enemy.ComponentsStore.Attackable.AttackDistance;
        }


        protected override void OnEnable()
        {
            _navMeshAgent.ResetPath();
            _navMeshAgent.stoppingDistance = _indentFromTarget;
        }


        protected override void Update()
        {
            Moving(_targetPosition.position);

            if (_navMeshAgent.remainingDistance <= _indentFromTarget)
            {
                if (!_isCameAttackPosition.Value)
                    _isCameAttackPosition.Value = true;
            }
            else
            {
                if (_isCameAttackPosition.Value)
                    _isCameAttackPosition.Value = false;
            }
        }


        public void Moving(Vector3 targetPosition)
        {
            _navMeshAgent.SetDestination(targetPosition);
        }


        protected override void OnDestroy() => Dispose();


        private void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }


    }
}