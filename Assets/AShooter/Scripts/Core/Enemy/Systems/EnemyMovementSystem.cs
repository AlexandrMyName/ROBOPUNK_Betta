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
        private Transform _playerTransform;
        private float _indentFromTarget;
        private Enemy _enemy;
        private ReactiveProperty<bool> _isCameAttackPosition;


        public EnemyMovementSystem(float indentFromTarget)
        {
            _indentFromTarget = indentFromTarget;
        }

        protected override void Awake(IGameComponents components)
        {
            _navMeshAgent = components.BaseObject.GetComponent<NavMeshAgent>();
            _enemy = components.BaseObject.GetComponent<Enemy>();
            _isCameAttackPosition = _enemy.IsCameAttackPosition;
            _playerTransform = _enemy.PlayerTransform;

#if UNITY_EDITOR
            if (_navMeshAgent == null)
            {
                Debug.LogError($"NavMeshAgent not found on Enemy object - {components.BaseObject.name}");
                return;
            }
#endif
        }


        protected override void OnEnable()
        {
            _navMeshAgent.ResetPath();
            _navMeshAgent.stoppingDistance = _indentFromTarget;
        }


        protected override void Update()
        {
            Moving(_playerTransform.position);

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


        private void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }


    }

}