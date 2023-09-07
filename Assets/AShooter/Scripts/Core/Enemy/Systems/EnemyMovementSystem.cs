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
        private Transform _targetTransform;
        private bool _readyToMeleeAttack;


        protected override void Awake(IGameComponents components)
        {
            var enemy = components.BaseObject.GetComponent<Enemy>();
            _targetTransform = enemy.playerTransform;
            enemy.IsReadyToMeleeAttack.Subscribe(OnReadyToMeleeAttack);

            _navMeshAgent = components.BaseObject.GetComponent<NavMeshAgent>();

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
        }


        protected override void Update()
        {
            if (_readyToMeleeAttack)
                Stay();
            else 
                Moving(_targetTransform.position);
        }

        private void Stay()
        {
            _navMeshAgent.isStopped = true;
        }

        public void Moving(Vector3 targetPosition)
        {
            if (_navMeshAgent.isStopped)
                _navMeshAgent.isStopped = false;

            if (_navMeshAgent.isActiveAndEnabled)
                _navMeshAgent.SetDestination(targetPosition);
        }


        private void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }


        private void OnReadyToMeleeAttack(bool val)
        {
            _readyToMeleeAttack = val;
        }

    }

}