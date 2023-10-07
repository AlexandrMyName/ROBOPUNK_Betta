using Abstracts;
using System;
using UnityEngine;
using User;
using Random = UnityEngine.Random;


namespace Core
{

    public class PickUpItem : MonoBehaviour
    {

        private IWeaponStorage _weaponStorage;
        private PickUpItemType _pickUpItemType;
        private WeaponType _weaponType;
        private int _bulletCount;
        private int _bulletMinCount = 5;
        private int _bulletMaxCount = 10;

        public WeaponType WeaponType { get { return _weaponType; } set {  _weaponType = value; } }
        public PickUpItemType PickUpItemType { get { return _pickUpItemType; } set { _pickUpItemType = value; } }


        private void Start()
        {
            SetComponents();

            _bulletCount = Random.Range(_bulletMinCount, _bulletMaxCount);
        }


        private void OnTriggerEnter(Collider unit)
        {
            if (unit.TryGetComponent(out IPlayer player))
            {
                _weaponStorage = player.ComponentsStore.WeaponStorage;

                Raise();
                Destroy(gameObject);
            }
        }


        private void SetComponents()
        {
            //Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
            //rigidbody.useGravity = true;
            //rigidbody.freezeRotation = true;
            //rigidbody.AddForce(gameObject.transform.forward * 10, ForceMode.Impulse);

            Outline outline = gameObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = (_pickUpItemType == PickUpItemType.Weapon) ? Color.blue : Color.white;
            outline.OutlineWidth = (_pickUpItemType == PickUpItemType.Weapon) ? 2 : 1;
        }


        private void Raise()
        {
            if (_pickUpItemType == PickUpItemType.Weapon)
            {
                _weaponStorage.WeaponState.PickUpWeapon.Value = _weaponStorage.Weapons[_weaponType] as IRangeWeapon;

                ChangeWeapon();
            }

            if ((_pickUpItemType == PickUpItemType.Bullet) || (_pickUpItemType == PickUpItemType.Weapon))
            {
                var weapon = _weaponStorage.Weapons[_weaponType] as IRangeWeapon;
                weapon.LeftPatronsCount.Value = Math.Clamp(weapon.LeftPatronsCount.Value + _bulletCount, weapon.LeftPatronsCount.Value, weapon.ClipSize);
            }
        }


        private void ChangeWeapon()
        {
            foreach (var weapon in _weaponStorage.Weapons)
            {
                weapon.Value.WeaponObject.SetActive(false);
            }

            _weaponStorage.Weapons[_weaponType].WeaponObject.SetActive(true);
        }


    }
}
