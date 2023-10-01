using Abstracts;
using System.Collections.Generic;
using System;
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
        private WeaponAbilityItemView _pickUpWeaponItem;
        private WeaponAbilityItemView _explosionAbilityItem;

        private List<IDisposable> _disposables = new();
        private List<IDisposable> _pickUpWeaponDisposables = new();


        public void InitWeapons(IWeaponStorage weaponStorage)
        {
            _meleeWeaponItem = CreateWeaponView(weaponStorage.Weapons[0], _weaponAbilityView.MeleeWeaponContainer, "Shift");
            _mainWeaponItem = CreateWeaponView(weaponStorage.WeaponState.MainWeapon.Value, _weaponAbilityView.MainWeaponContainer, "ЛКМ");
            _pickUpWeaponItem = CreateWeaponView(weaponStorage.WeaponState.PickUpWeapon.Value, _weaponAbilityView.PickUpWeaponContainer, "ПКМ");

            WeaponSubscribe(weaponStorage);
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


        private void WeaponSubscribe(IWeaponStorage weaponStorage)
        {
            MeleeWeaponSubscribe(weaponStorage, _meleeWeaponItem);
            RangeWeaponSubscribe(weaponStorage.WeaponState.MainWeapon, _mainWeaponItem, _disposables, false);
            RangeWeaponSubscribe(weaponStorage.WeaponState.PickUpWeapon, _pickUpWeaponItem, _pickUpWeaponDisposables, true);
        }


        private void RangeWeaponSubscribe(ReactiveProperty<IRangeWeapon> weapon, WeaponAbilityItemView weaponItemView, List<IDisposable> disposables, bool clearDisposables)
        {
            if (weapon.Value is IRangeWeapon rangeWeapon)
            {
                if (clearDisposables)
                    disposables.ForEach(d => d.Dispose());

                disposables.AddRange(new List<IDisposable> {
                    weapon.Subscribe(_ => weaponItemView.SetItemIcon(rangeWeapon.WeaponIcon)),
                    rangeWeapon.LeftPatronsCount.Subscribe(count => weaponItemView.SetPatronsCount(count)),
                    rangeWeapon.IsReloadProcessing.Subscribe(isReload => OnReload(weaponItemView, rangeWeapon.ReloadTime, isReload))
                });
            }
        }


        private void MeleeWeaponSubscribe(IWeaponStorage weaponStorage, WeaponAbilityItemView weaponItemView)
        {
            var weapon = weaponStorage.Weapons[0];

            if (weapon is IMeleeWeapon meleeWeapon)
            {
                _disposables.Add(
                    weaponStorage.WeaponState.IsMeleeWeaponPressed.Subscribe(isPressed => { weaponItemView.gameObject.SetActive(isPressed); })
                );
            }
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


    }
}
