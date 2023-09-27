using System;
using System.Collections.Generic;
using Abstracts;
using UniRx;
using Zenject;


namespace Core
{

    public sealed class PlayerWeaponSystem : BaseSystem, IDisposable
    {

        [Inject] private IInput _input;
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

            ChangeWeapon(1);
            
            _disposables.AddRange(new List<IDisposable>{
                    _input.MeleeHold.AxisOnChange.Subscribe(b => HandleMeleeButtonPressed(b)),
                    _input.WeaponFirst.AxisOnChange.Subscribe(_ => HandleWeaponChangePress(1)),
                    _input.WeaponSecond.AxisOnChange.Subscribe(_ => HandleWeaponChangePress(2)),
                    _input.WeaponThird.AxisOnChange.Subscribe(_ => HandleWeaponChangePress(3)),
                    
                    _input.LeftClick.AxisOnChange.Subscribe(_ =>
                    {
                        if (!_weaponStorage.WeaponState.IsMeleeWeaponPressed.Value)
                            HandleWeaponChangePress(1);
                    }),
                    
                    _input.RightClick.AxisOnChange.Subscribe(_ =>
                    {
                        if (!_weaponStorage.WeaponState.IsMeleeWeaponPressed.Value)
                            HandleWeaponChangePress(2);
                    })
                }
            );

            _weaponStorage.WeaponState.CurrentWeapon.Value = _weaponStorage.Weapons[1];

        }


        private void HandleWeaponChangePress(int weaponId)
        {
            
            if (!_weaponStorage.WeaponState.CurrentWeapon.Value.WeaponId.Equals(weaponId))
                ChangeWeapon(weaponId);
        }


        private void ChangeWeapon(int id)
        {
            foreach (var weapon in _weaponStorage.Weapons)
            {
                weapon.Value.WeaponObject.SetActive(false);
            }

            _weaponStorage.Weapons[id].WeaponObject.SetActive(true);
            _weaponStorage.WeaponState.CurrentWeapon.Value = _weaponStorage.Weapons[id];
        }
        
        
        private void HandleMeleeButtonPressed(bool isPressing)
        {
            if (isPressing)
            {
                _isAlredyFalsed = false;
                HandleWeaponChangePress(0);
                _weaponStorage.WeaponState.IsMeleeWeaponPressed.Value = true;
            }
            else
            {
                if (!_isAlredyFalsed)
                {
                    _weaponStorage.WeaponState.IsMeleeWeaponPressed.Value = false;
                    HandleWeaponChangePress(1);
                }

                _isAlredyFalsed = true;
            }
        }


        public void Dispose() => _disposables.ForEach(d => d.Dispose());

    }
}
