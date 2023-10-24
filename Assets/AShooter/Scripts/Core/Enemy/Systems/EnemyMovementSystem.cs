using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Abstracts;
using System;
using UniRx;


namespace Core
{

    public class EnemyMovementSystem : BaseSystem, IDisposable
    {

        private List<IDisposable> _disposables = new();
        private NavMeshAgent _navMeshAgent;
        private Transform _targetPosition;
        private float _indentFromTarget;

        private IGameComponents _components;    
        private IEnemy _enemy;
        private IAnimatorIK _animator;

        private ReactiveProperty<bool> _isCameAttackPosition;


        public EnemyMovementSystem(Transform targetPosition)
        {
            _targetPosition = targetPosition;
        }


        protected override void Awake(IGameComponents components)
        {

            _components = components;

            _navMeshAgent = components.BaseObject.GetComponent<NavMeshAgent>();
            _enemy = components.BaseObject.GetComponent<IEnemy>();
            _animator = components.BaseObject.GetComponent<IAnimatorIK>();
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

            if (!_navMeshAgent.isActiveAndEnabled) return;

            Moving(_targetPosition.position);

            if (_navMeshAgent.remainingDistance <= _indentFromTarget)
            {
                if (!_isCameAttackPosition.Value)
                {
                    _isCameAttackPosition.Value = true;
                    _enemy.EnemyState = DTO.EnemyState.Stunned;
                }
            }
            else
            {
                if (_isCameAttackPosition.Value)
                {
                    _isCameAttackPosition.Value = false;
                    _enemy.EnemyState = DTO.EnemyState.Walk;
                }
            }


            if(_animator != null)
            {
                _animator.SetAimingAnimation(_isCameAttackPosition.Value, default);


                if (_enemy.EnemyState == DTO.EnemyState.Stunned)
                {
                 
                    _navMeshAgent.updateRotation = true;
                    _navMeshAgent.updateUpAxis = true;
                }
            }
        }


        public void Moving(Vector3 targetPosition)
        {
            _navMeshAgent.SetDestination(targetPosition);
        }

         
        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }
    }
}