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

    public sealed class PlayerWeaponSystem : BaseSystem, IDisposable
    {

        [Inject] private IInput _input;
        [Inject] private WeaponAbilityPresenter _weaponAbilityPresenter;

        private List<IDisposable> _disposables = new();
        private IWeaponStorage _weaponStorage;
        private WeaponState _weaponState;
        private PlayerAnimatorIK _animatorIK;
        private bool _isAlredyFalsed;


        protected override void Awake(IGameComponents components)
        {
          
            var weaponContainer = components.BaseObject.GetComponent<Player>().WeaponContainer;
            _weaponStorage = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.WeaponStorage;
            _animatorIK = components.BaseObject.GetComponent<PlayerAnimatorIK>();
            _weaponStorage.InitializeWeapons(weaponContainer, _animatorIK);
            _weaponState = _weaponStorage.WeaponState;
            _weaponAbilityPresenter.Init(_weaponStorage);
        }


        protected override void Start()
        {
            _disposables.AddRange(new List<IDisposable>{
                    _input.MeleeHold.AxisOnChange.Subscribe(b => HandleMeleeButtonPressed(b)),
                    _input.WeaponFirst.AxisOnChange.Subscribe(_ => HandleWeaponChangePress(WeaponType.Rifle)),
                    _input.WeaponSecond.AxisOnChange.Subscribe(_ => HandleWeaponChangePress(WeaponType.Shotgun)),
                    _input.WeaponThird.AxisOnChange.Subscribe(_ => HandleWeaponChangePress(WeaponType.RocketLauncher)),
                    
                    _input.RightClick.AxisOnChange.Subscribe(pressed =>
                    {
                        if (pressed && !_weaponState.IsMeleeWeaponPressed.Value)
                            HandleWeaponChangePress(WeaponType.Pistol);
                    }),
                    
                    _input.LeftClick.AxisOnChange.Subscribe(pressed =>
                    {
                        if (pressed && !_weaponState.IsMeleeWeaponPressed.Value && _weaponState.MainWeapon.Value != null)
                            HandleWeaponChangePress(_weaponState.MainWeapon.Value.WeaponType);
                    }),
                    _weaponState.MainWeapon.Subscribe(weapon => {
                        if (weapon != null)
                            HandleWeaponChangePress(weapon.WeaponType);
                        })
                }
            );

            _weaponState.SecondaryWeapon.Value = _weaponStorage.Weapons[WeaponType.Pistol] as IRangeWeapon;
            _weaponState.MeleeWeapon.Value = _weaponStorage.Weapons[WeaponType.Sword] as IMeleeWeapon;

            _weaponAbilityPresenter.MeleeWeaponSubscribe(_weaponState.MeleeWeapon.Value);
            _weaponAbilityPresenter.SecondaryWeaponSubscribe(_weaponState.SecondaryWeapon.Value);
            
            ChangeWeapon(WeaponType.Pistol);
        }


        private void HandleWeaponChangePress(WeaponType weaponType)
        {
            if (!_weaponState.CurrentWeapon.WeaponType.Equals(weaponType))
            {
                ChangeWeapon(weaponType);
            }
        }


        private void ChangeWeapon(WeaponType weaponType)
        {
            foreach (var weapon in _weaponStorage.Weapons)
            {
                weapon.Value.WeaponObject.SetActive(false);
            }

            _weaponStorage.Weapons[weaponType].WeaponObject.SetActive(true);
            _weaponState.CurrentWeapon = _weaponStorage.Weapons[weaponType];

            SetAnimation(weaponType);
            ResolveWeaponStateByType(weaponType);
             
        }


        private void ResolveWeaponStateByType(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Sword:
                    _weaponState.MeleeWeapon.Value = _weaponStorage.Weapons[WeaponType.Sword] as IMeleeWeapon;
                    
                    break;
                
                case WeaponType.Pistol:
                    _weaponState.SecondaryWeapon.Value = _weaponStorage.Weapons[WeaponType.Pistol] as IRangeWeapon;
                    
                    break;
                    
                default:
                    _weaponState.MainWeapon.Value = _weaponStorage.Weapons[weaponType] as IRangeWeapon;
                    _weaponAbilityPresenter.MainWeaponSubscribe(_weaponState.MainWeapon.Value);
                    break;
            }
        }



        private void SetAnimation(WeaponType weaponType)
        => _animatorIK._rigAnimator.Play(weaponType.ToString() + "Aim",0 );
          


        private void HandleMeleeButtonPressed(bool isPressing)
        {
            if (isPressing)
            {
                _isAlredyFalsed = false;
                HandleWeaponChangePress(WeaponType.Sword);
                _weaponState.IsMeleeWeaponPressed.Value = true;
            }
            else
            {
                if (!_isAlredyFalsed)
                {
                    HandleWeaponChangePress(WeaponType.Pistol);
                    _weaponState.IsMeleeWeaponPressed.Value = false;
                }

                _isAlredyFalsed = true;
            }
        }


        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            foreach (var weapon in _weaponStorage.Weapons)
            {
                weapon.Value.Dispose();
            }
        }


    }
}
