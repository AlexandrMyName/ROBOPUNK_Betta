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
        private Enemy _enemy;


        protected override void Awake(IGameComponents components)
        {
            _enemy = components.BaseObject.GetComponent<Enemy>();
            _enemy.IsCameAttackPosition.Subscribe(SetPositionReadiness);

            var shootDisposable = Observable
                .Interval(TimeSpan.FromSeconds(GameLoopManager.EnemyAttackFrequency))
                .TakeUntilDestroy(_enemy)
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
            Vector3 directionToPlayer = (_enemy.PlayerTransform.position - _enemy.transform.position).normalized;

            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var rb = primitive.AddComponent<Rigidbody>();
            primitive.transform.position = _enemy.transform.position + directionToPlayer*2;
            rb.velocity = directionToPlayer * 10f;

            UnityEngine.Object.Destroy(primitive, 5f);
        }


    }

} 