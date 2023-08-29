using System;
using System.Collections.Generic;
using Abstracts;
using UniRx;
using UnityEngine;
using Zenject;


namespace Core
{

    public sealed class PlayerShootSystem : BaseSystem
    {

        [Inject] private IInput _input;
        [Inject] private IWeapon _weapon;

        private IGameComponents _components;
        private Camera _camera;
        private Vector3 _mousePosition;
        private List<IDisposable> _disposables = new();


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _camera = _components.MainCamera;
            Debug.Log($"Initialized shoot system! ({components.BaseObject.name})");
        }


        protected override void Start()
        {
            _disposables.AddRange(new List<IDisposable>{
                _input.LeftClick.AxisOnChange.Subscribe(OnLeftClick),
                _input.MousePosition.AxisOnChange.Subscribe(OnMousePositionChanged)}
            );
        }


        protected override void Update()
        {
            var ray = _camera.ScreenPointToRay(_mousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _weapon.LayerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);
            }
        }


        protected override void OnDestroy()
        {
            _disposables.ForEach(d => d.Dispose());
        }


        private void OnLeftClick(bool isClicked)
        {
            if (isClicked)
            {
                FindTarget();
            }
        }


        private void OnMousePositionChanged(Vector3 position)
        {
            _mousePosition = position;
        }


        private void FindTarget()
        {
            var ray = _camera.ScreenPointToRay(_mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _weapon.LayerMask))
            {
                var hitCollider = hitInfo.collider;

                if (hitCollider.TryGetComponent(out IAttackable unit))
                {
                    unit.TakeDamage(_weapon.Damage);
                }
                else
                {
                    Debug.Log($"{hitCollider} IAttackable is not found");
                }

                SpawnParticleEffectOnHit(hitInfo);
            }
        }


        private void SpawnParticleEffectOnHit(RaycastHit hitInfo)
        {
            if (_weapon.EffectPrefab != null)
            {
                var hitEffectRotation = Quaternion.LookRotation(hitInfo.normal);
                var hitEffect = GameObject.Instantiate(_weapon.EffectPrefab, new Vector3(hitInfo.point.x, 1f, hitInfo.point.z), hitEffectRotation);

                GameObject.Destroy(hitEffect.gameObject, _weapon.EffectDestroyDelay);
            }
        }

    }
}
