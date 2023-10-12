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

        
        public void Init(IWeaponStorage weaponStorage)
        {
            _weaponStorage = weaponStorage;
            _weaponState = weaponStorage.WeaponState;
        }


        public void InitWeapons()
        {
            foreach (var weaponPair in _weaponStorage.Weapons)
            {
                var weapon = weaponPair.Value;

                if (weapon.WeaponType == WeaponType.Sword)
                {
                    _meleeWeaponItem = CreateWeaponView(weapon, _weaponAbilityView.MeleeWeaponContainer, "Shift");
                    WeaponSubscribe(weapon);
                }

                if (weapon.WeaponType == WeaponType.Pistol)
                {
                    _secondaryWeaponItem = CreateWeaponView(weapon, _weaponAbilityView.SecondaryWeaponContainer, "ПКМ");
                    WeaponSubscribe(weapon);
                }


                if (weapon.WeaponType != WeaponType.Pistol && weapon.WeaponType != WeaponType.Sword)
                {
                    _mainWeaponItem = CreateWeaponView(weapon, _weaponAbilityView.MainWeaponContainer, "ЛКМ");
                    WeaponSubscribe(weapon);
                }
            }
        }


        public void InitAbilities(IAbility ability)
        {
            _explosionAbilityItem = CreateAbilityView(ability, _weaponAbilityView.ExplosionAbilityContainer, "Q");

            AbilitySubscribe(ability, _explosionAbilityItem);

            _weaponAbilityView.Show();
        }


        private WeaponAbilityItemView CreateWeaponView(IWeapon weapon, Transform itemContainer, string buttonName)
        {
            var itemObject = Instantiate(_itemViewPrefab, itemContainer);

            WeaponAbilityItemView itemView = itemObject.GetComponent<WeaponAbilityItemView>();

            var patronsCount = 0;
            if (weapon is IRangeWeapon rangeWeapon)
                patronsCount = rangeWeapon.LeftPatronsCount.Value;
            else
                patronsCount = -1;

            itemView.SetItemIcon(weapon.WeaponIcon);
            itemView.SetPatronsCount(patronsCount);
            itemView.SetButtonName(buttonName);

            return itemView;
        }


        private WeaponAbilityItemView CreateAbilityView(IAbility ability, Transform itemContainer, string buttonName)
        {
            var itemObject = Instantiate(_itemViewPrefab, itemContainer);

            WeaponAbilityItemView itemView = itemObject.GetComponent<WeaponAbilityItemView>();
            itemView.SetItemIcon(ability.ExplosionIcon);
            itemView.SetPatronsCount(-1);
            itemView.SetButtonName(buttonName);

            return itemView;
        }


        private void WeaponSubscribe(IWeapon weapon)
        {
            if (weapon.WeaponType == WeaponType.Sword)
            {
                MeleeWeaponSubscribe(weapon as IMeleeWeapon, _meleeWeaponItem);
            }

            if (weapon.WeaponType == WeaponType.Pistol)
            {
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

            if (weapon.WeaponType != WeaponType.Pistol && weapon.WeaponType != WeaponType.Sword)
            {
                _weaponState.MainWeapon.Subscribe(_ =>
                {
                    RangeWeaponSubscribe(
                        _weaponState.MainWeapon, 
                        _mainWeaponItem, 
                        _pickUpWeaponDisposables, 
                        true);
                }).AddTo(_disposables);
            }
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


        private void MeleeWeaponSubscribe(IMeleeWeapon weapon, WeaponAbilityItemView weaponItemView)
        {
            _disposables.Add(
                _weaponStorage.WeaponState.IsMeleeWeaponPressed.Subscribe(isPressed =>
                {
                    weaponItemView.gameObject.SetActive(isPressed);
                })
            );
        }
        


        private void AbilitySubscribe(IAbility ability, WeaponAbilityItemView weaponItemView)
        {
            _disposables.Add(
                ability.IsReady.Subscribe(isReady =>
                {
                    if (!isReady)
                        OnReload(weaponItemView, ability.UsageTimeout, !isReady);
                })
            );
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
