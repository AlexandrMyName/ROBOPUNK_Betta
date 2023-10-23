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

    public sealed class PlayerShootSystem : BaseSystem, IDisposable
    {

        [Inject] private IInput _input;
        [Inject] private WeaponState _weaponState;

        private IGameComponents _components;
        private Camera _camera;
        private Vector3 _mousePosition;
        private List<IDisposable> _disposables = new();

        private IRangeWeapon _mainRangeWeapon;
        private IRangeWeapon _secondaryRangeWeapon;

        private bool _isMainWeaponAvailable;
        private bool _isPickUpWeaponAvailable;

        private AudioSource _audioSource;


        private bool _isSecondaryWeaponAvailable;
        
        
        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _camera = _components.MainCamera;
            _audioSource = components.BaseObject.GetComponent<AudioSource>();
        }


        protected override void Start()
        {
            _disposables.AddRange(new List<IDisposable>{
                _input.MousePosition.AxisOnChange.Subscribe(OnMousePositionChanged),
                
                _weaponState.MainWeapon.Subscribe(weapon => UpdateMainWeapon(weapon)),
                _weaponState.SecondaryWeapon.Subscribe(weapon => UpdateSecondaryWeapon(weapon)),
                
                _input.RightClick.AxisOnChange.Subscribe(pressed =>
                {
                    if (pressed) 
                        TryShootPerform(_secondaryRangeWeapon);
                }),
                _input.LeftClick.AxisOnChange.Subscribe(pressed =>
                {
                    if (pressed)
                        TryShootPerform(_mainRangeWeapon);
                })
            });
        }


        private void UpdateMainWeapon(IRangeWeapon rangeWeapon)
        {
            if (rangeWeapon != null)
            {
                _mainRangeWeapon = rangeWeapon;
                _isMainWeaponAvailable = true;
            }
        }


        private void UpdateSecondaryWeapon(IRangeWeapon rangeWeapon)
        {
            if (rangeWeapon != null)
            {
                _secondaryRangeWeapon = rangeWeapon;
                _isSecondaryWeaponAvailable = true;
            }
        }
        
        
        protected override void Update()
        {
            DrawDebugRayToMousePosition();
            UpdateWeaponLaser();
        }

        
        private void UpdateWeaponLaser()
        {
            if (_isMainWeaponAvailable && _mainRangeWeapon.WeaponObject.activeSelf)
                _mainRangeWeapon.Laser.Update();
            
            if (_isSecondaryWeaponAvailable && _secondaryRangeWeapon.WeaponObject.activeSelf)
                _secondaryRangeWeapon.Laser.Update();
        }


        private void TryShootPerform(IRangeWeapon weapon)
        {
            if (weapon == null) return;
            
            if ((_isMainWeaponAvailable || _isSecondaryWeaponAvailable) && !_weaponState.IsMeleeWeaponPressed.Value)
            {
                if (weapon.IsShootReady)
                {
                    if (weapon.LeftPatronsCount.Value > 0)
                    {
                        weapon.Shoot(_mousePosition);
                        PlaySound(_audioSource, weapon);
                    }
                    else
                        if ((weapon.WeaponType == WeaponType.Pistol) || (weapon.TotalPatrons.Value != 0))
                            weapon.ProcessReload();
                }
            }
        }


        private void PlaySound(AudioSource audioSource, IRangeWeapon weapon)
        {
            if (audioSource != null)
            {
                var soundModelType = weapon.WeaponType switch
                {
                    WeaponType.Pistol => SoundModelType.Weapon_Pistol,
                    WeaponType.Shotgun => SoundModelType.Weapon_Shotgun,
                    WeaponType.RocketLauncher => SoundModelType.Weapon_RocketLouncher,
                    WeaponType.Rifle => SoundModelType.Weapon_Rifle,
                    _ => SoundModelType.None
                };

                if (soundModelType != SoundModelType.None)
                {
                    AudioClip audioClip = SoundManager.Config.GetSound(SoundType.Damage, soundModelType);

                    if (audioClip != null)
                        audioSource.PlayOneShot(audioClip);
                }
            }
        }


        private void OnMousePositionChanged(Vector3 position) => _mousePosition = position;
       

        private void DrawDebugRayToMousePosition()
        {
            var ray = _camera.ScreenPointToRay(_mousePosition);
            
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _secondaryRangeWeapon.LayerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);
            }
        }
        

        public void Dispose() => _disposables.ForEach(d => d.Dispose());

        
    }
}
