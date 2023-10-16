using Abstracts;
using System.Collections.Generic;
using System;
using Core;
using Core.DTO;
using UniRx;
using UnityEngine;


namespace User
{

    public class WeaponAbilityPresenter : MonoBehaviour, IDisposable
    {

        [SerializeField] private WeaponAbilityView _weaponAbilityView;
        [SerializeField] private GameObject _itemViewPrefab;

        private WeaponAbilityItemView _meleeWeaponItem;
        private WeaponAbilityItemView _mainWeaponItem;
        private WeaponAbilityItemView _secondaryWeaponItem;
        private WeaponAbilityItemView _explosionAbilityItem;

        private List<IDisposable> _disposables = new();
        private List<IDisposable> _pickUpWeaponDisposables = new();

        private IWeaponStorage _weaponStorage;
        private WeaponState _weaponState;


        private void Start()
        {
            _meleeWeaponItem = CreateAbilityView(_weaponAbilityView.MeleeWeaponContainer, "Shift");
            _mainWeaponItem = CreateAbilityView(_weaponAbilityView.MainWeaponContainer, "LMB");
            _secondaryWeaponItem = CreateAbilityView(_weaponAbilityView.SecondaryWeaponContainer, "RMB");
            _explosionAbilityItem = CreateAbilityView(_weaponAbilityView.ExplosionAbilityContainer, "Q");
        }

        
        public void Init(IWeaponStorage weaponStorage)
        {
            _weaponStorage = weaponStorage;
            _weaponState = weaponStorage.WeaponState;
        }


        public void SubscribeAbility(IAbility ability)
        {
            _explosionAbilityItem.SetItemIcon(ability.ExplosionIcon);
            _explosionAbilityItem.SetPatronsCount(-1);
            
            _disposables.Add(
                ability.IsReady.Subscribe(isReady =>
                {
                    if (!isReady)
                        OnReload(_explosionAbilityItem, ability.UsageTimeout, !isReady);
                })
            );
        }
        
        
        public void MeleeWeaponSubscribe(IMeleeWeapon weapon)
        {
            _meleeWeaponItem.SetItemIcon(weapon.WeaponIcon);
            _meleeWeaponItem.SetPatronsCount(-1);
            
            _disposables.Add(
                _weaponStorage.WeaponState.IsMeleeWeaponPressed.Subscribe(isPressed =>
                {
                    _meleeWeaponItem.gameObject.SetActive(isPressed);
                })
            );
        }


        public void SecondaryWeaponSubscribe(IRangeWeapon weapon)
        {
            var patronsCount = weapon.LeftPatronsCount.Value;
            
            _secondaryWeaponItem.SetItemIcon(weapon.WeaponIcon);
            _secondaryWeaponItem.SetPatronsCount(patronsCount);
            
            _weaponState.SecondaryWeapon.Subscribe(_ =>
            {
                RangeWeaponSubscribe(
                    _weaponState.SecondaryWeapon, 
                    _secondaryWeaponItem, 
                    _disposables,
                    false
                );
            }).AddTo(_disposables);
        }


        public void MainWeaponSubscribe(IRangeWeapon weapon)
        {
            var patronsCount = weapon.LeftPatronsCount.Value;
            
            _mainWeaponItem.SetItemIcon(weapon.WeaponIcon);
            _mainWeaponItem.SetPatronsCount(patronsCount);
            
            _weaponState.MainWeapon.Subscribe(_ =>
            {
                RangeWeaponSubscribe(
                    _weaponState.MainWeapon, 
                    _mainWeaponItem, 
                    _pickUpWeaponDisposables,
                    true
                );
            }).AddTo(_disposables);
        }
        
        
        private WeaponAbilityItemView CreateAbilityView(Transform itemContainer, string buttonName)
        {
            var itemObject = Instantiate(_itemViewPrefab, itemContainer);
            WeaponAbilityItemView itemView = itemObject.GetComponent<WeaponAbilityItemView>();
            
            itemView.SetButtonName(buttonName);
            return itemView;
        }


        private void RangeWeaponSubscribe(ReactiveProperty<IRangeWeapon> rangeWeapon, WeaponAbilityItemView weaponItemView, List<IDisposable> disposables, bool clearDisposables)
        {
            if (clearDisposables)
                disposables.ForEach(d => d.Dispose());

            weaponItemView.SetItemIcon(rangeWeapon.Value.WeaponIcon);

            disposables.AddRange(new List<IDisposable> {
                rangeWeapon.Value.LeftPatronsCount.Subscribe(count => weaponItemView.SetPatronsCount(count)),
                rangeWeapon.Value.IsReloadProcessing.Subscribe(isReload => OnReload(weaponItemView, rangeWeapon.Value.ReloadTime, isReload))
            });
        }

        
        public void OnReload(WeaponAbilityItemView weaponItemView, float reloadTime, bool isReload)
        {
            if (isReload)
            {
                var tick = 0.1f;

                weaponItemView.ReloadIcon.gameObject.SetActive(true);

                _disposables.Add(
                    Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(tick))
                        .Take((int)(reloadTime / tick))
                        .Subscribe(_ =>
                        {
                            weaponItemView.ReloadIcon.fillAmount -= 1 / reloadTime * tick;
                        }, () =>
                        {
                            weaponItemView.ReloadIcon.gameObject.SetActive(false);
                            weaponItemView.ReloadIcon.fillAmount = 1;
                        }
                    )
                );
            }
        }


        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
            _pickUpWeaponDisposables.ForEach(d => d.Dispose());
        }

        
        private void OnDestroy() => Dispose();
        
        
    }
}
