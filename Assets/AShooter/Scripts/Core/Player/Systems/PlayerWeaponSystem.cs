using System;
using System.Collections.Generic;
using System.Linq;
using Abstracts;
using Core.DTO;
using UniRx;
using UnityEngine;
using User;
using Zenject;


namespace Core
{

    public sealed class PlayerWeaponSystem : BaseSystem
    {

        [Inject] private IInput _input;
        [Inject] private WeaponState _weaponState;
        [Inject] private readonly List<WeaponConfig> _weaponConfigs;

        private IGameComponents _components;
        private List<IDisposable> _disposables = new();
        
        private Player _player;
        private readonly Dictionary<int, IWeapon> _weapons = new();


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _player = components.BaseObject.GetComponent<Player>();
            Debug.Log($"Initialized Player Weapon System! ({components.BaseObject.name})");
        }


        protected override void Start()
        {
            InitializeWeapons(_weaponConfigs);
            _disposables.AddRange(new List<IDisposable>{
                    _input.WeaponFirst.AxisOnChange.Subscribe(_ => HandleWeaponChangePress(1)),
                    _input.WeaponSecond.AxisOnChange.Subscribe(_ => HandleWeaponChangePress(2)),
                    _input.WeaponThird.AxisOnChange.Subscribe(_ => HandleWeaponChangePress(3))
                }
            );
        }


        protected override void Update()
        {
        }
        
        
        protected override void OnDestroy()
        {
            _disposables.ForEach(d => d.Dispose());
        }


        private void InitializeWeapons(List<WeaponConfig> configs)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                var config = configs[i];
                
                switch (config.WeaponType)
                {
                    case WeaponType.Pistol:
                        _weapons[config.WeaponId] = PistolInit(config);
                        break;
                    case WeaponType.Shotgun:
                        _weapons[config.WeaponId] = ShotgunInit(config);
                        break;
                    case WeaponType.RocketLauncher:
                        _weapons[config.WeaponId] = RocketLauncherInit(config);
                        break;

                }
            }

            _weaponState.CurrentWeapon.Value = _weapons[1];
        }


        private IWeapon PistolInit(WeaponConfig config)
        {
            var pistolObject = GameObject.Instantiate(config.WeaponObject, _player.WeaponContainer);
            return new Pistol(
                config.WeaponId,
                pistolObject,
                null,
                config.WeaponType,
                config.Damage,
                config.ClipSize,
                config.LeftPatronsCount,
                config.ReloadTime,
                config.ShootDistance,
                config.ShootSpeed,
                config.FireSpread,
                config.LayerMask,
                config.Effect,
                config.EffectDestroyDelay);
        }


        private IWeapon ShotgunInit(WeaponConfig config)
        {
            var shotgunObject = GameObject.Instantiate(config.WeaponObject, _player.WeaponContainer);
            shotgunObject.SetActive(false);
            return new Shotgun(
                config.WeaponId,
                shotgunObject,
                null,
                config.WeaponType,
                config.Damage,
                config.ClipSize,
                config.LeftPatronsCount,
                config.ReloadTime,
                config.ShootDistance,
                config.ShootSpeed,
                config.FireSpread,
                config.SpreadFactor,
                config.LayerMask,
                config.Effect,
                config.EffectDestroyDelay);
        }


        private IWeapon RocketLauncherInit(WeaponConfig config)
        {
            var rocketLauncherObject = GameObject.Instantiate(config.WeaponObject, _player.WeaponContainer);
            rocketLauncherObject.SetActive(false);
            return new RocketLauncher(
                config.WeaponId,
                rocketLauncherObject,
                config.ProjectileObject,
                config.ProjectileForce,
                config.WeaponType,
                config.Damage,
                config.ClipSize,
                config.LeftPatronsCount,
                config.ReloadTime,
                config.ShootDistance,
                config.ShootSpeed,
                config.FireSpread,
                config.LayerMask,
                config.Effect,
                config.EffectDestroyDelay);
        }


        private void HandleWeaponChangePress(int weaponId)
        {
            Debug.Log($"PRESSED WEAPON CHANGE BUTTON - [{weaponId}]");
            
            if (!_weaponState.CurrentWeapon.Value.WeaponId.Equals(weaponId))
                ChangeWeapon(weaponId);
        }


        private void ChangeWeapon(int id)
        {
            foreach (var weapon in _weapons)
            {
                weapon.Value.WeaponObject.SetActive(false);
            }
            
            _weapons[id].WeaponObject.SetActive(true);
            _weaponState.CurrentWeapon.Value = _weapons[id];
        }


    }
}
