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
        [Inject] private List<WeaponConfig> _weaponConfigs;

        private IGameComponents _components;
        private List<IDisposable> _disposables = new();
        
        private Player _player;
        private Dictionary<int, IWeapon> _weapons = new();


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _player = components.BaseObject.GetComponent<Player>();
            
            InitializeWeapons(_weaponConfigs);
            
            Debug.Log($"Initialized Player Weapon System! ({components.BaseObject.name})");
        }


        protected override void Start()
        {
            _disposables.AddRange(new List<IDisposable>{
                    
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
                        _weapons[config.WeapoId] = PistolInit(config);
                        break;
                }
            }

            _weaponState.CurrentWeapon.Value = _weapons[1];
        }


        private IWeapon PistolInit(WeaponConfig config)
        {
            var pistolObject = GameObject.Instantiate(config.WeaponPrefab, _player.WeaponContainer);
            return new Pistol(
                pistolObject, 
                config.LayerMask, 
                config.EffectPrefab, 
                config.Damage, 
                config.EffectDestroyDelay);
        }


    }
}
