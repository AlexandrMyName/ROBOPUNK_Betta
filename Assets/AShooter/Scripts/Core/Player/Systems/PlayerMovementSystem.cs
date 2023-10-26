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

            _animatorIK.RootAnimator.SetFloat("Forward", localMove.z, 0.5f , 3 * Time.deltaTime);
        }


        //private Vector3 GetRelativePos(Vector3 dir)
        //{
        //    Vector3 relativeDir = GetPlanePosition(Input.mousePosition) - _playerTransform.position;
        //    relativeDir.Normalize();
        //    relativeDir.y = 0;
        //    return relativeDir;
        //}


        //private Vector3 GetPlanePosition(Vector3 mousePos)
        //{

        //    var ray = _components.MainCamera.ScreenPointToRay(mousePos);

        //    if (Physics.Raycast(ray, out var s))
        //    {
        //        return s.point;
        //    }
        //    if (_plane.Raycast(ray, out float enterPoint))
        //    {

        //        return ray.GetPoint(enterPoint);
        //    }
        //    return Vector3.zero;
        //}



        protected override void FixedUpdate()
        {

            _movable.Rigidbody.velocity = new Vector3(
                _direction.x * _movable.Speed.Value,
                _movable.Rigidbody.velocity.y, 
                _direction.z * _movable.Speed.Value
                );
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