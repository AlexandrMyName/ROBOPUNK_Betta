using System;
using System.Collections.Generic;
using Abstracts;
using Core.DTO;
using UniRx;
using UnityEngine;
using User;
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

        private float _reloadTimer;

        
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
                _weaponState.CurrentWeapon.Subscribe(weapon => { UpdateCurrentWeapon(weapon); })
            });
        }

        
        private void UpdateCurrentWeapon(IWeapon weapon)
        {
            _currentWeapon = weapon;
            _reloadTimer = _currentWeapon?.ReloadTime ?? 0.0f;
        }


        protected override void Update()
        {
            DrawDebugRayToMousePosition();

            if (_weaponState.IsNeedReload)
                ProcessReload(Time.deltaTime);
        }
        
        
        protected override void OnDestroy()
        {
            _disposables.ForEach(d => d.Dispose());
        }


        private void OnLeftClick(bool isClicked)
        {
            if (isClicked)
            {
                if (_currentWeapon.LeftPatronsCount > 0)
                {
                    _currentWeapon.Shoot(_components.BaseTransform, _camera, _mousePosition);
                }
                else
                {
                    _weaponState.IsNeedReload = true;
                }
            }
        }


        private void OnMousePositionChanged(Vector3 position)
        {
            _mousePosition = position;
        }


        private void DrawDebugRayToMousePosition()
        {
            var ray = _camera.ScreenPointToRay(_mousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _currentWeapon.LayerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);
            }
        }


        private void ProcessReload(float deltaTime)
        {
            if (_reloadTimer >= 0)
            {
                _reloadTimer -= deltaTime;
            }
            else
            {
                _currentWeapon.Reload();
                _weaponState.IsNeedReload = false;
                _reloadTimer = _currentWeapon.ReloadTime;
            }
        }


    }
}
