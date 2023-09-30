using System;
using System.Collections.Generic;
using Abstracts;
using Core.DTO;
using UniRx;
using UnityEngine;
using Zenject;


namespace Core
{

    public sealed class PlayerShootSystem : BaseSystem, IDisposable
    {

        [Inject] private IInput _input;
        [Inject] private WeaponState _weaponState;

        private IGameComponents _components;
        private Camera _camera;
        private Vector3 _mousePosition;
        private List<IDisposable> _disposables = new();

        private IRangeWeapon _currentRangeWeapon;
        
        
        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _camera = _components.MainCamera;
        }


        protected override void Start()
        {

            _disposables.AddRange(new List<IDisposable>{
                _input.LeftClick.AxisOnChange.Subscribe(_ => TryShootPerform()),
                _input.RightClick.AxisOnChange.Subscribe(_ => TryShootPerform()),
                _input.MousePosition.AxisOnChange.Subscribe(OnMousePositionChanged),
                _weaponState.CurrentWeapon.Subscribe(weapon => { UpdateCurrentWeapon(weapon); })
            });
        }

        
        private void UpdateCurrentWeapon(IWeapon rangeWeapon)
        {

            if (rangeWeapon is IRangeWeapon weapon)
                _currentRangeWeapon = weapon;
            else
                _currentRangeWeapon = null;
        }
        
        
        protected override void Update()
        {
            DrawDebugRayToMousePosition();
            if (_currentRangeWeapon != null)
                _currentRangeWeapon.Laser.Update();
        }

        
        private void TryShootPerform()
        {

            if (_currentRangeWeapon != null)
            {
                if (_currentRangeWeapon.IsShootReady)
                {
                    if (_currentRangeWeapon.LeftPatronsCount.Value > 0)
                        _currentRangeWeapon.Shoot(_components.BaseTransform, _camera, _mousePosition);
                    else
                        _currentRangeWeapon.ProcessReload();
                }
            }
        }


        private void OnMousePositionChanged(Vector3 position) => _mousePosition = position;
       

        private void DrawDebugRayToMousePosition()
        {

            if (_currentRangeWeapon != null)
            {
                var ray = _camera.ScreenPointToRay(_mousePosition);

                if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _currentRangeWeapon.LayerMask))
                {
                    Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);
                }
            }
        }


        public void Dispose() => _disposables.ForEach(d => d.Dispose());

    }
}
