using Abstracts;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;


namespace Core
{

    public sealed class PlayerShootSystem : BaseSystem
    {

        [Inject] private IInput _input;
        [Inject] private Weapon _weapon;

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
            _disposables.AddRange(new ArrayList(){
                _input.LeftClick.AxisOnChange.Subscribe(OnLeftClick),
                _input.MousePosition.AxisOnChange.Subscribe(OnMousePositionChanged)}
            );
        }


        protected override void Update()
        {
            var ray = _camera.ScreenPointToRay(_mousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _weapon.layerMask))
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
                PerformRaycast();
            }
        }


        private void OnMousePositionChanged(Vector3 position)
        {
            _mousePosition = position;
        }


        private void PerformRaycast()
        {
            var ray = _camera.ScreenPointToRay(_mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _weapon.layerMask))
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
            if (_weapon.effectPrefab != null)
            {
                var hitEffectRotation = Quaternion.LookRotation(hitInfo.normal);
                var hitEffect = GameObject.Instantiate(_weapon.effectPrefab, hitInfo.point, hitEffectRotation);

                GameObject.Destroy(hitEffect.gameObject, _weapon.effectDestroyDelay);
            }
        }

#if UNITY_EDITOR
        //private void OnDrawGizmosSelected()
        //{
        //    var ray = new Ray(_components.BaseTransform.position, _components.BaseTransform.forward);
        //    DrawRaycast(ray);
        //}

        //private void DrawRaycast(Ray ray)
        //{
        //    if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _weapon.layerMask))
        //    {
        //        DrawRay(ray, hitInfo.point, hitInfo.distance, Color.red);
        //    }
        //}

        //private void DrawRay(Ray ray, Vector3 hitPosition, float distance, Color color)
        //{
        //    const float hitPointRadius = 0.15f;

        //    Debug.DrawRay(ray.origin, ray.direction * distance, color);

        //    Gizmos.color = color;
        //    Gizmos.DrawSphere(hitPosition, hitPointRadius);
        //}
#endif

    }
}
