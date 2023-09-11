using System;
using System.Collections.Generic;
using Abstracts;
using Core.DTO;
using UniRx;
using UnityEngine;
using Zenject;


namespace Core
{

    public sealed class PlayerShootSystem : BaseSystem
    {

        [Inject] private IInput _input;
        [Inject] private WeaponState _weaponState;

        private IGameComponents _components;
        private Camera _camera;
        private Vector3 _mousePosition;
        private List<IDisposable> _disposables = new();

        private IWeapon _currentWeapon;


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
                _input.MousePosition.AxisOnChange.Subscribe(OnMousePositionChanged),
                _weaponState.CurrentWeapon.Subscribe(weapon => _currentWeapon = weapon)
            });
        }


        protected override void Update()
        {
            // DrawDebugRayToMousePosition();
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

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _currentWeapon.LayerMask))
            {
                var hitCollider = hitInfo.collider;

                if (hitCollider.tag == "Player") return;
                if (hitCollider.TryGetComponent(out IAttackable unit))
                {
                    Debug.Log($"Found target [{unit}]");
                    unit.TakeDamage(_currentWeapon.Damage);
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
            if (_currentWeapon.Effect != null)
            {
                var hitEffectRotation = Quaternion.LookRotation(hitInfo.normal);
                
                var hitEffect = GameObject.Instantiate(
                    _currentWeapon.Effect,
                    hitInfo.point, 
                    hitEffectRotation);

                GameObject.Destroy(hitEffect.gameObject, _currentWeapon.EffectDestroyDelay);
            }
        }
        
        
        private void DrawDebugRayToMousePosition()
        {
            var ray = _camera.ScreenPointToRay(_mousePosition);
        
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _currentWeapon.LayerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);
            }
        }
        

    }
}
