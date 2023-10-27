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
        private IGameComponents _components;
        private List<IDisposable> _disposables = new();

        private PlayerAnimatorIK _animatorIK;

        private Transform _playerTransform;

        private Plane _plane;

        private Vector3 _v = Vector3.zero;
        private Vector3 _h = Vector3.zero;
        private Vector3 _movement = Vector3.zero;

        private float _reactiveJumpHeight = 4.5f;
        

        protected override void Awake(IGameComponents components)
        {

             _components = components;
            _animator = components.BaseObject.GetComponent<AnimatorIK>();
            _playerTransform = components.BaseTransform;
            _movable = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Movable;
            _animatorIK = components.BaseObject.GetComponent<PlayerAnimatorIK>();
        }


        protected override void Start()
        {

            _disposables.AddRange(new List<IDisposable>{
                    _input.Horizontal.AxisOnChange.Subscribe(value => _direction.x = value),
                    _input.Vertical.AxisOnChange.Subscribe(value => _direction.z = value) 
            });
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

            if (Input.GetKey(KeyCode.Space))
            {
                _movable.Rigidbody.transform.position
                    = Vector3.Lerp(_movable.Rigidbody.transform.position,
                    new Vector3(_movable.Rigidbody.transform.position.x,
                    _reactiveJumpHeight,
                    _movable.Rigidbody.transform.position.z), 10 * Time.deltaTime);

                _movable.Rigidbody.velocity = new Vector3(_movable.Rigidbody.velocity.x, 0, _movable.Rigidbody.velocity.z);

                _animatorIK.RootAnimator.SetBool("ReactiveJump", true);

                _animatorIK.JetPackEffects.ForEach(effect => effect.gameObject.SetActive(true));

            }
            else
            {
                _animatorIK.RootAnimator.SetBool("ReactiveJump", false);
                _animatorIK.JetPackEffects.ForEach(effect => effect.gameObject.SetActive(false));
            }
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