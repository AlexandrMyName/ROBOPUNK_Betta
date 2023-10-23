using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;


namespace Core
{

    public class MP3PlayerSystem : BaseSystem, IDisposable
    {

        [Inject] private IInput _input;

        private ReactiveProperty<int> CurrentClipIndex;
        private IMP3PlayerView _view;
        private List<IDisposable> _disposables = new();
        private Player _player;
        private AudioSource _audioSource;
        private AudioClip[] _audioClips;
        private bool _isPaused;


        protected override void Awake(IGameComponents components)
        {
            _player = components.BaseObject.GetComponent<Player>();

            _view = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.MP3PlayerView;
            _view.Show();

            _audioSource = SoundManager.AudioSource;
            CurrentClipIndex = new ReactiveProperty<int>(-1);
            _isPaused = false;
        }


        protected override void Start()
        {
            _audioClips = RandomPermutation(SoundManager.MP3PlayerConfig.AudioClips.ToArray());

            _input.MP3Player.AxisOnChange.Subscribe(_ => {
                if (SoundManager.IsPlaying)
                    Pause();
                else
                    Play();
                
                _player.Headset.gameObject.SetActive(SoundManager.IsPlaying);

            }).AddTo(_disposables);

            CurrentClipIndex.Subscribe(index => {
                if (SoundManager.IsPlaying)
                {
                    if (index >= _audioClips.Length)
                        CurrentClipIndex.Value = 0;
                    else
                    {
                        _audioSource.clip = _audioClips[index];
                        _audioSource.Play();
                        _view.ChangeText(_audioSource.clip.name);
                        _view.Ticker = true;
                    }
                }
            }).AddTo(_disposables);
        }


        protected override void Update()
        {
            if (SoundManager.IsPlaying && !_isPaused && !_audioSource.isPlaying)
                CurrentClipIndex.Value++;
        }


        private void Play()
        {
            _isPaused = false;
            SoundManager.IsPlaying = true;

            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();

                if (_audioSource.clip != null)
                {
                    _view.ChangeText(_audioSource.clip.name);
                    _view.Ticker = true;
                }
            }
        }


        private void Pause()
        {
            _isPaused = true;
            SoundManager.IsPlaying = false;
            _audioSource.Pause();
            _view.Ticker = false;
        }


        private AudioClip[] RandomPermutation(AudioClip[] audioClips)
        {
            System.Random random = new System.Random();
            var audioClipsCount = audioClips.Length;

            while (audioClipsCount > 1)
            {
                audioClipsCount--;
                var i = random.Next(audioClipsCount + 1);
                var temp = audioClips[i];
                audioClips[i] = audioClips[audioClipsCount];
                audioClips[audioClipsCount] = temp;
            }

            return audioClips;
        }


        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());


    }
}
