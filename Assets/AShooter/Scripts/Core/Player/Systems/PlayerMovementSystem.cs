using System;
using System.Collections.Generic;
using Abstracts;
using UniRx;
using UnityEngine;
using Zenject;


namespace Core 
{

    public sealed class PlayerMovementSystem : BaseSystem, IDisposable
    {

        [Inject] private IInput _input;

        private AnimatorIK _animator;
        
        private Vector3 _direction;
        private IMovable _movable;
        private IJetPackView _jetPackView;
        private IGameComponents _components;
        private List<IDisposable> _disposables = new();

        private PlayerAnimatorIK _animatorIK;

        private Transform _playerTransform;
 
        private Vector3 _v = Vector3.zero;
        private Vector3 _h = Vector3.zero;
        private Vector3 _movement = Vector3.zero;

        private float _currentJumpValue = 0;
        private bool _isRegenerationJump;

         
        protected override void Awake(IGameComponents components)
        {

             _components = components;
            _animator = components.BaseObject.GetComponent<AnimatorIK>();
            _playerTransform = components.BaseTransform;
            _movable = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Movable;
            _jetPackView = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.JetPackView;
            _animatorIK = components.BaseObject.GetComponent<PlayerAnimatorIK>();
        }


        protected override void Start()
        {

            _disposables.AddRange(new List<IDisposable>{
                    _input.Horizontal.AxisOnChange.Subscribe(value => _direction.x = value),
                    _input.Vertical.AxisOnChange.Subscribe(value => _direction.z = value) 
            });
            _currentJumpValue = _movable.JumpTime;
        }

        
        protected override void Update()
        {

            _movable.MoveDirection = _direction;
             
            _v = _direction.z * _components.MainCamera.transform.up;
            _h = _direction.x * _components.MainCamera.transform.right;

            _v.y = 0;
            _h.y = 0;

            _movement = (_h + _v).normalized;

            Vector3 localMove = _playerTransform.InverseTransformDirection(_movement);

             
                _animatorIK.RootAnimator.SetFloat("Right", localMove.x, 0.5f, 3 * Time.deltaTime);

                _animatorIK.RootAnimator.SetFloat("Forward", localMove.z, 0.5f, 3 * Time.deltaTime);
        }

         
        protected override void FixedUpdate()
        {

            _movable.Rigidbody.velocity = new Vector3(
                _direction.x * _movable.Speed.Value,
                _movable.Rigidbody.velocity.y,
                _direction.z * _movable.Speed.Value
                );

            UpdateJumpState();

        }


        private void UpdateJumpState()
        {

            if (Input.GetKey(KeyCode.Space) && _currentJumpValue >= 0)
            {

                _movable.Rigidbody.transform.position
                    = Vector3.Lerp(_movable.Rigidbody.transform.position,
                    new Vector3(_movable.Rigidbody.transform.position.x,
                    _movable.JumpHeight,
                    _movable.Rigidbody.transform.position.z), 10 * Time.deltaTime);

                _movable.Rigidbody.velocity = new Vector3(_movable.Rigidbody.velocity.x, 0, _movable.Rigidbody.velocity.z);

                _currentJumpValue -= Time.deltaTime * 2f;
                _jetPackView.RefreshValue(_currentJumpValue, _movable.JumpTime);

                _animatorIK.RootAnimator.SetBool("ReactiveJump", true);

                _animatorIK.JetPackEffects.ForEach(effect => effect.gameObject.SetActive(true));
            }

            else if (_currentJumpValue > 0)
            {
                if (_currentJumpValue > _movable.JumpTime) return;

                _animatorIK.RootAnimator.SetBool("ReactiveJump", false);
                _animatorIK.JetPackEffects.ForEach(effect => effect.gameObject.SetActive(false));
                _currentJumpValue += Time.deltaTime;
                _jetPackView.RefreshValue(_currentJumpValue, _movable.JumpTime);

            }
            else
            {
                _animatorIK.RootAnimator.SetBool("ReactiveJump", false);
                _animatorIK.JetPackEffects.ForEach(effect => effect.gameObject.SetActive(false));
                if (_isRegenerationJump) return;
                Observable.Timer(TimeSpan.FromSeconds(2f)).Subscribe(_ => UpdateJumpTime()).AddTo(_disposables);
                _isRegenerationJump = true;
            }
        }


        private void UpdateJumpTime()
        {
            Debug.Log("S");
            _currentJumpValue = 1f;
            _jetPackView.RefreshValue(_currentJumpValue, _movable.JumpTime);
            _isRegenerationJump = false;
        }


        protected override void OnDestroy() => _disposables.ForEach(d => d.Dispose());


        public void Dispose()
        {

            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
            _movable.Rigidbody.isKinematic = true;
        }

    }

}