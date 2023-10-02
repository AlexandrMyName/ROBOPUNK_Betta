using Abstracts;
using Core.DTO;
using System.Collections.Generic;
using UniRx;
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
        
        
        public WeaponStorage()
        { 
            Weapons = new Dictionary<WeaponType, IWeapon>();
            _camera = Camera.main;
        }


        public void InitializeWeapons(Transform weaponContainer)
        {

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


        private IWeapon CreateRangeWeapon(WeaponConfig config)
        {
            var weaponObject = GameObject.Instantiate(config.WeaponObject, _weaponContainer);
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
                _camera
            );
        }


        private IWeapon CreateMeleeWeapon(WeaponConfig config)
        {
            var meleeWeaponObject = GameObject.Instantiate(config.WeaponObject, _weaponContainer);
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

        
    }
}