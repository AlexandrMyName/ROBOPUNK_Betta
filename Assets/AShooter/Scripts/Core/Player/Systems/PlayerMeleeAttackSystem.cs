using System;
using System.Collections.Generic;
using Abstracts;
using Core.DTO;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using User;
using Zenject;


namespace Core
{

    public sealed class PlayerMeleeAttackSystem : BaseSystem, IDisposable
    {

        [Inject] private IInput _input;
        [Inject] private WeaponState _weaponState;

        private IGameComponents _components;
        private List<IDisposable> _disposables = new();

        private IMeleeWeapon _currentMeleeWeapon;
        
        
        protected override void Awake(IGameComponents components)
        {
            _components = components;
        }


        protected override void Start()
        {
            _disposables.AddRange(new List<IDisposable>{
                _input.LeftClick.AxisOnChange.Subscribe(_ =>
                {
                    if (_weaponState.IsMeleeWeaponPressed.Value)
                        TryAttackPerform();
                }),
                
                _weaponState.MeleeWeapon.Subscribe(weapon => { UpdateMeleeWeapon(weapon); })
            });
        }
        
        
        protected override void OnDrawGizmos()
        {
            if (_currentMeleeWeapon != null)
            {
                ((MeleeWeapon) _currentMeleeWeapon).DrawBoxCast();
            }
        }
        
      
        private void UpdateMeleeWeapon(IMeleeWeapon meleeWeapon)
        {
            _currentMeleeWeapon = meleeWeapon;
        }


        private void TryAttackPerform()
        {
            if (_currentMeleeWeapon.IsAttackReady)
                _currentMeleeWeapon.Attack();
        }

        
        public void Dispose() => _disposables.ForEach(d => d.Dispose());
        

    }
}
