using System;
using System.Collections.Generic;
using Abstracts;
using Core.DTO;
using UniRx;
using UnityEngine;
using User;
using Zenject;


namespace Core
{

    public sealed class PlayerMeleeAttackSystem : BaseSystem, IDisposable
    {

        [Inject] private IInput _input;
        [Inject] private WeaponState _weaponState;
        [Inject] private WeaponAbilityPresenter _weaponAbilityPresenter;

        private IGameComponents _components;
        private Camera _camera;
        private List<IDisposable> _disposables = new();

        private IMeleeWeapon _currentMeleeWeapon;

        private AudioSource _audioSource;
        private AudioClip _hitAudioClip;


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _camera = _components.MainCamera;
         
          
        }


        protected override void Start()
        {
            _disposables.AddRange(new List<IDisposable>{
                _input.LeftClick.AxisOnChange.Subscribe(pressed =>
                {
                    if (pressed && _weaponState.IsMeleeWeaponPressed.Value)
                        TryAttackPerform();
                }),
                
                _weaponState.MeleeWeapon.Subscribe(weapon => { UpdateMeleeWeapon(weapon); })
            });

            _audioSource = _components.BaseObject.GetComponent<AudioSource>();
            _hitAudioClip = SoundManager.Config.GetSound(SoundType.Damage, SoundModelType.Weapon_Sword);
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
            {
                _currentMeleeWeapon.Attack();
                PlaySound(_audioSource, _hitAudioClip);
            }
        }


        private void PlaySound(AudioSource audioSource, AudioClip audioClip)
        {
            if ((audioSource != null) && (audioClip != null))
                audioSource.PlayOneShot(audioClip);
        }


        public void Dispose() => _disposables.ForEach(d => d.Dispose());
        

    }
}
