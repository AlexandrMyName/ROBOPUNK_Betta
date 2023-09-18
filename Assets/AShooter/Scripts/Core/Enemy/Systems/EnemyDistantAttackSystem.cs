using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Core
{

    public class EnemyDistantAttackSystem : BaseSystem
    {

        private List<IDisposable> _disposables = new();
        private bool _isPositionReadiness;
        private IEnemy _enemy;
        IGameComponents _componentsInPrefab;

        protected override void Awake(IGameComponents components)
        {
            _componentsInPrefab = components;
            _enemy = components.BaseObject.GetComponent<IEnemy>();
            _enemy.ComponentsStore.Attackable.IsCameAttackPosition.Subscribe(SetPositionReadiness);

            var shootDisposable = Observable
                .Interval(TimeSpan.FromSeconds(GameLoopManager.EnemyAttackFrequency))
                .TakeUntilDestroy(_enemy as Component)
                .Subscribe(_ => DoShoot());
        }


        private void SetPositionReadiness(bool val)
        {

            _isPositionReadiness = val;
        }


        private void DoShoot()
        {
            if (_isPositionReadiness)
            {
                ThrowPrimitive();
            }
                
        }


        void ThrowPrimitive()
        {
            Vector3 directionToPlayer = (_enemy.PlayerTransform.position - _componentsInPrefab.BaseTransform.position).normalized;

            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var rb = primitive.AddComponent<Rigidbody>();
            primitive.transform.position = _componentsInPrefab.BaseTransform.position + directionToPlayer*2;
            rb.velocity = directionToPlayer * 10f;

            UnityEngine.Object.Destroy(primitive, 5f);
        }


    }

} 