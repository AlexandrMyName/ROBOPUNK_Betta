using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.AI;


namespace Core
{

    public class EnemyDistantAttackSystem : BaseSystem
    {

        private List<IDisposable> _disposables = new();
        private bool _isPositionReadiness;
        private IEnemy _enemy;
        private IAnimatorIK _animator;
        private IGameComponents _components;
        private Transform _targetPosition;
        private float _attackFrequency;
        private NavMeshAgent _navMeshAgent ;

        public EnemyDistantAttackSystem(Transform targetPosition)
        {
            _targetPosition = targetPosition;
        }


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _enemy = components.BaseObject.GetComponent<IEnemy>();
            _animator = components.BaseObject.GetComponent<IAnimatorIK>();
            _navMeshAgent = components.BaseObject.GetComponent<NavMeshAgent>();
            _enemy.ComponentsStore.Attackable.IsCameAttackPosition.Subscribe(SetPositionReadiness);
            _attackFrequency = _enemy.ComponentsStore.Attackable.AttackFrequency;

            var shootDisposable = Observable
                .Interval(TimeSpan.FromSeconds(_attackFrequency))
                .TakeUntilDestroy(_enemy as Component)
                .Where(_ => (_isPositionReadiness))
                .Subscribe(_ => DoShoot())
                .AddTo(_disposables);
        }


        private void SetPositionReadiness(bool val) => _isPositionReadiness = val;
        

        private void DoShoot()
        {

            if (!_components.BaseObject.activeSelf 
                ||
                !_navMeshAgent.isActiveAndEnabled) return;

            if (_isPositionReadiness)
            {
                if(_animator != null)
                {
                    _animator.ShootIK();
                }
                else
                {
                    ThrowPrimitive();
                }
            }

        }


        [Obsolete]
        void ThrowPrimitive()
        {
            Vector3 directionToPlayer = (_targetPosition.position - _components.BaseTransform.position).normalized;

            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var rb = primitive.AddComponent<Rigidbody>();
            primitive.transform.position = _components.BaseTransform.position + directionToPlayer*2;
            rb.velocity = directionToPlayer * 10f;

            UnityEngine.Object.Destroy(primitive, 5f);
        }


    }

} 