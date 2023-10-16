using Abstracts;
using System;
using UnityEngine;
using User;
using Random = UnityEngine.Random;


namespace Core
{

    public class PickUpItem : MonoBehaviour, IPickUpItem
    {

        private PickUpItemType _pickUpItemType;
        private WeaponType _weaponType;

        private int _patronsCount;
        private int _patronsMinCount = 10;
        private int _patronsMaxCount = 15;

        public WeaponType WeaponType { get { return _weaponType; } set {  _weaponType = value; } }
        public PickUpItemType PickUpItemType { get { return _pickUpItemType; } set { _pickUpItemType = value; } }


        private void Start()
        {
            SetComponents();

            _patronsCount = Random.Range(_patronsMinCount, _patronsMaxCount);
        }


        private void SetComponents()
        {
            Outline outline = gameObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = (_pickUpItemType == PickUpItemType.Weapon) ? Color.blue : Color.white;
            outline.OutlineWidth = (_pickUpItemType == PickUpItemType.Weapon) ? 2 : 1;
        }


        public void Raise(IWeaponStorage weaponStorage)
        {
            if (_pickUpItemType == PickUpItemType.Weapon)
            {
                weaponStorage.WeaponState.MainWeapon.Value = weaponStorage.Weapons[_weaponType] as IRangeWeapon;
            }

            if ((_pickUpItemType == PickUpItemType.Bullet) || (_pickUpItemType == PickUpItemType.Weapon))
            {
                var weapon = weaponStorage.Weapons[_weaponType] as IRangeWeapon;
                weapon.TotalPatrons.Value = Math.Clamp(weapon.TotalPatrons.Value + _patronsCount, _patronsCount, weapon.TotalPatronsMaxCount);
            }

            Destroy(gameObject);
        }


    }
}
