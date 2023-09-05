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
        private Dictionary<int, IWeapon> _weapons;


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _player = components.BaseObject.GetComponent<Player>();
            Debug.Log($"Initialized Player Weapon System! ({components.BaseObject.name})");
        }


        protected override void Start()
        {
            _disposables.AddRange(new List<IDisposable>{
                    
                }
            );

            InitializeWeapons();
        }


        protected override void Update()
        {
        }
        
        
        protected override void OnDestroy()
        {
            _disposables.ForEach(d => d.Dispose());
        }


        private void InitializeWeapons()
        {
            
        }


    }
}
