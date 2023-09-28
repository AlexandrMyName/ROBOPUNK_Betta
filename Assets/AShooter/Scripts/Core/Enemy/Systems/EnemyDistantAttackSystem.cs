using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


namespace Core
{

    public class EnemyDistantAttackSystem : BaseSystem
    {

        private List<IDisposable> _disposables = new();
        private bool _isPositionReadiness;
        private IEnemy _enemy;
        private IGameComponents _components;
        private Vector3 _targetPosition;
        private float _attackFrequency;

        public EnemyDistantAttackSystem(Vector3 targetPosition, float attackFrequency)
        {
            _targetPosition = targetPosition;
            _attackFrequency = attackFrequency;
        }


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _enemy = components.BaseObject.GetComponent<IEnemy>();
            _enemy.ComponentsStore.Attackable.IsCameAttackPosition.Subscribe(SetPositionReadiness);

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

            if (!_components.BaseObject.activeSelf) return;

            if (_isPositionReadiness) ThrowPrimitive();

        }


        void ThrowPrimitive()
        {
            Vector3 directionToPlayer = (_targetPosition - _components.BaseTransform.position).normalized;

            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var rb = primitive.AddComponent<Rigidbody>();
            primitive.transform.position = _components.BaseTransform.position + directionToPlayer*2;
            rb.velocity = directionToPlayer * 10f;

            UnityEngine.Object.Destroy(primitive, 5f);
        }


    }

} 