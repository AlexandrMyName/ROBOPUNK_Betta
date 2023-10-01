using System;
using System.Collections.Generic;
using Abstracts;
using UniRx;
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
        private bool _isAlredyFalsed;


        protected override void Awake(IGameComponents components)
        {
          
            var weaponContainer = components.BaseObject.GetComponent<Player>().WeaponContainer;
            _weaponStorage = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.WeaponStorage;

            _weaponStorage.InitializeWeapons(weaponContainer);
        }


        protected override void Start()
        {

            ChangeWeapon(WeaponType.Pistol);
            
            _disposables.AddRange(new List<IDisposable>{
                    _input.MeleeHold.AxisOnChange.Subscribe(b => HandleMeleeButtonPressed(b)),
                    _input.WeaponFirst.AxisOnChange.Subscribe(_ => HandleWeaponChangePress(WeaponType.Pistol)),
                    _input.WeaponSecond.AxisOnChange.Subscribe(_ => HandleWeaponChangePress(WeaponType.Shotgun)),
                    _input.WeaponThird.AxisOnChange.Subscribe(_ => HandleWeaponChangePress(WeaponType.RocketLauncher)),
                    
                    _input.LeftClick.AxisOnChange.Subscribe(_ =>
                    {
                        if (!_weaponStorage.WeaponState.IsMeleeWeaponPressed.Value)
                            HandleWeaponChangePress(WeaponType.Pistol);
                    }),
                    
                    _input.RightClick.AxisOnChange.Subscribe(_ =>
                    {
                        if (!_weaponStorage.WeaponState.IsMeleeWeaponPressed.Value)
                            HandleWeaponChangePress(WeaponType.Shotgun);
                    })
                }
            );

            _weaponStorage.WeaponState.CurrentWeapon.Value = _weaponStorage.Weapons[WeaponType.Pistol];
            _weaponStorage.WeaponState.MainWeapon.Value = _weaponStorage.Weapons[WeaponType.Pistol];
            _weaponStorage.WeaponState.PickUpWeapon.Value = _weaponStorage.Weapons[WeaponType.Shotgun];

            _weaponAbilityPresenter.InitWeapons(_weaponStorage);
        }


        private void HandleWeaponChangePress(WeaponType weaponType)
        {
            
            if (!_weaponStorage.WeaponState.CurrentWeapon.Value.WeaponType.Equals(weaponType))
                ChangeWeapon(weaponType);
        }


        private void ChangeWeapon(WeaponType weaponType)
        {
            foreach (var weapon in _weaponStorage.Weapons)
            {
                weapon.Value.WeaponObject.SetActive(false);
            }

            _weaponStorage.Weapons[weaponType].WeaponObject.SetActive(true);
            _weaponStorage.WeaponState.CurrentWeapon.Value = _weaponStorage.Weapons[weaponType];

            UpdatePickUpWeapon();
        }


        private void UpdatePickUpWeapon()
        {
            var currentWeapon = _weaponStorage.WeaponState.CurrentWeapon.Value;

            if ((currentWeapon.WeaponType != WeaponType.Pistol) && (currentWeapon.WeaponType != WeaponType.Sword))
            {
                _weaponStorage.WeaponState.PickUpWeapon.Value = currentWeapon;
            }
        }


        private void HandleMeleeButtonPressed(bool isPressing)
        {
            if (isPressing)
            {
                _isAlredyFalsed = false;
                HandleWeaponChangePress(WeaponType.Sword);
                _weaponStorage.WeaponState.IsMeleeWeaponPressed.Value = true;
            }
            else
            {
                if (!_isAlredyFalsed)
                {
                    _weaponStorage.WeaponState.IsMeleeWeaponPressed.Value = false;
                    HandleWeaponChangePress(WeaponType.Pistol);
                }

                _isAlredyFalsed = true;
            }
        }


        public void Dispose() => _disposables.ForEach(d => d.Dispose());


    }
}
