using Abstracts;
using Core.DTO;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using User;
using Zenject;


namespace Core
{

    public class WeaponStorage : IWeaponStorage
    {

        [Inject] public WeaponState WeaponState { get; private set; }
        [Inject] public List<WeaponConfig> WeaponConfigs { get; private set; }
        
        public Dictionary<WeaponType, IWeapon> Weapons { get; }

        private Camera _camera;
        private Transform _weaponContainer;
        private PlayerAnimatorIK _animatorIK;


        public WeaponStorage()
        { 
            Weapons = new Dictionary<WeaponType, IWeapon>();
            _camera = Camera.main;
        }


        public void InitializeWeapons(Transform weaponContainer, PlayerAnimatorIK animatorIK)
        {

            if(_animatorIK == null)
            {
                _animatorIK = animatorIK;
            }

            _weaponContainer = weaponContainer;
            
            for (int i = 0; i < WeaponConfigs.Count; i++)
            {
                var config = WeaponConfigs[i];

                switch (config.WeaponType)
                {
                    case WeaponType.Sword:
                        Weapons[WeaponType.Sword] = CreateMeleeWeapon(config);
                        break;
                    case WeaponType.Pistol:
                        Weapons[WeaponType.Pistol] = CreateRangeWeapon(config);
                        break;
                    case WeaponType.Shotgun:
                        Weapons[WeaponType.Shotgun] = CreateRangeWeapon(config);
                        break;
                    case WeaponType.RocketLauncher:
                        Weapons[WeaponType.RocketLauncher] = CreateRangeWeapon(config);
                        break;
                    case WeaponType.Rifle:
                        Weapons[WeaponType.Rifle] = CreateRangeWeapon(config);
                        break;
                }
            }
        }


        private GameObject FindWeapon(WeaponConfig config)
        => _animatorIK.WeaponData.Find(weap => weap.Type == config.WeaponType).WeaponHolder.gameObject;
         
        private IWeapon CreateRangeWeapon(WeaponConfig config)
        {
            var weaponObject = FindWeapon(config);
            weaponObject.SetActive(false);

            return new Weapon(
                config.WeaponId,
                weaponObject,
                config.WeaponIcon,
                config.ProjectileObject,
                config.ProjectileForce,
                config.WeaponType,
                config.Damage,
                config.ClipSize,
                new ReactiveProperty<int>(config.LeftPatronsCount),
                config.TotalPatronsMaxCount,
                config.ReloadTime,
                config.ShootDistance,
                config.ShootSpeed,
                config.FireSpread,
                config.SpreadFactor,
                config.LayerMask,
                config.MuzzleEffect,
                config.MuzzleEffectDestroyDelay,
                config.Effect,
                config.EffectDestroyDelay,
                config.FakeWeaponObject,
                config.FakeBulletsObject,
                _camera
            );
        }


        private IWeapon CreateMeleeWeapon(WeaponConfig config)
        {
            var meleeWeaponObject = FindWeapon(config);
            meleeWeaponObject.SetActive(false);

            return new MeleeWeapon(
                config.WeaponId,
                meleeWeaponObject,
                config.WeaponIcon,
                config.WeaponType,
                config.Damage,
                config.LayerMask,
                config.Effect,
                config.EffectDestroyDelay,
                config.ShootSpeed
            );
        }


        public void GetPickUpItem(PickUpItemModel pickUpItemModel)
        {
            var config = pickUpItemModel.WeaponConfig;

            var pickUpObject = pickUpItemModel.PickUpItemType switch
            {
                PickUpItemType.Weapon => config.FakeWeaponObject,
                PickUpItemType.Bullet => config.FakeBulletsObject,
                _ => config.WeaponObject
            };

            var parentPoint = pickUpItemModel.ParentPoint;
            var weaponObject = GameObject.Instantiate(pickUpObject, new Vector3(parentPoint.x, parentPoint.y + 0.5f, parentPoint.z + 3), Quaternion.identity);

            PickUpItem pickUpItem = weaponObject.AddComponent<PickUpItem>();
            pickUpItem.WeaponType = config.WeaponType;
            pickUpItem.PickUpItemType = pickUpItemModel.PickUpItemType;
        }


        public void UpgradeWeaponsStatesAccordingPlayerBaseStats(float baseDamageMultiplier, float baseAttackSpeedMultiplier)
        {
            foreach (var weaponPair in Weapons)
            {
                if (weaponPair.Key.Equals(WeaponType.Sword) || weaponPair.Key.Equals(WeaponType.Saw))
                {
                    var meleeWeapon = (IMeleeWeapon) weaponPair.Value;
                    meleeWeapon.Damage *= baseDamageMultiplier;
                    meleeWeapon.AttackTimeout -= (meleeWeapon.AttackTimeout * baseAttackSpeedMultiplier - meleeWeapon.AttackTimeout);
                }
                else
                {
                    var rangeWeapon = (IRangeWeapon) weaponPair.Value;
                    rangeWeapon.Damage *= baseDamageMultiplier;
                    rangeWeapon.ShootSpeed -= (rangeWeapon.ShootSpeed * baseAttackSpeedMultiplier - rangeWeapon.ShootSpeed);
                }
            }
        }

        
    }
}