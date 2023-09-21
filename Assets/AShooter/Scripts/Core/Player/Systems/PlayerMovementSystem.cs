using System;
using System.Collections.Generic;
using Abstracts;
using UniRx;
using UnityEngine;
using Zenject;


namespace Core 
{

    public sealed class PlayerMovementSystem : BaseSystem
    {

        [Inject] private IInput _input;

        private PlayerAnimator _animator;
        
        private Vector3 _direction;
        private IMovable _movable;
     
        private List<IDisposable> _disposables = new();


        protected override void Awake(IGameComponents components)
        {
             
            _animator = components.BaseObject.GetComponent<PlayerAnimator>();

            _movable = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Movable;
        }


        protected override void Start()
        {
            
            _disposables.AddRange(new List<IDisposable>{
                    _input.Horizontal.AxisOnChange.Subscribe(OnHorizontalChanged),
                    _input.Vertical.AxisOnChange.Subscribe(OnVerticalChanged)}
            );
        }

        protected override void Update() => _movable.MoveDirection = _direction;


        protected override void FixedUpdate()
        {

            _movable.Rigidbody.velocity = new Vector3(
                _direction.x * _movable.Speed.Value,
                _movable.Rigidbody.velocity.y, 
                _direction.z * _movable.Speed.Value
                );

        }


        protected override void OnDestroy() => _disposables.ForEach(d => d.Dispose());
         

        private void OnHorizontalChanged(float value) => _direction.x = value;
      

        private void OnVerticalChanged(float value) => _direction.z = value;

    }
}