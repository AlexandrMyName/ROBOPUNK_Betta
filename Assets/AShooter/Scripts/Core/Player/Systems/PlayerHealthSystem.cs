using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


namespace Core
{

    public class PlayerHealthSystem : BaseSystem, IDisposable
    {

        public IComponentsStore ComponentsStore { get; private set; }
        private IGameComponents _components;
        private IAttackable _attackable;
        private IPlayerHP _playerHP;
        private IDeathView _loseView;
        private IHealthView _healthView;
        private List<IDisposable> _disposables = new();
        private AudioClip _deathAudioClip;
        private AudioSource _audioSource;


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _attackable = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Attackable;
            _playerHP = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.PlayerHP;

            _healthView = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.Health;
            var currentHealth = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Attackable.Health;
            _disposables.Add(_attackable.Health.Subscribe(UpdateDisplay));
            _healthView.Show();
            _healthView.ChangeDisplay(currentHealth.Value,100);

            _loseView = _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.Death;
            _disposables.Add(_attackable.Health.Subscribe(DeathCheck));
             
        }


        protected override void Start()
        {

            _audioSource = _components.BaseObject.GetComponent<AudioSource>();
            _deathAudioClip = SoundManager.Config.GetSound(SoundType.Death, SoundModelType.Player);
        }


        public void Dispose()
        {

            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }


        private void UpdateDisplay(float healthValue) => _healthView.ChangeDisplay(healthValue, 100);//Add max health value 


        private void DeathCheck(float leftHealth)
        {

            if (leftHealth <= 0)
            {

                var playerRigidbody = _components.BaseObject.GetComponent<Rigidbody>();

                playerRigidbody.AddForce(Vector3.back * _playerHP.PunchForce, ForceMode.Impulse);
               
                _playerHP.IsAlive.Value = false;

                InputManager.DisableSystem();

                PlaySound(_audioSource, _deathAudioClip);

                _loseView.Show();

            }
           
        }


        private void PlaySound(AudioSource audioSource, AudioClip audioClip)
        {
            SoundManager.IsPlaying = false;
            SoundManager.AudioSource.clip = null;

            if ((audioSource != null) && (audioClip != null))
                audioSource.PlayOneShot(audioClip);
        }


    }
}

