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
        private IGameComponents _components;
        private PlayerAnimator _animator;
        private float _horizontal;
        private float _vertical;
        private Vector3 _direction;
        private IMovable _movable;
        
        
        private List<IDisposable> _disposables = new();


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _animator = components.BaseObject.GetComponent<PlayerAnimator>();
            Debug.Log($"Initialized move system! ({components.BaseObject.name})");
            if (_animator == null) Debug.LogWarning($"Player animator not found on {components.BaseObject.name}");
            _movable = _components.BaseObject.GetComponent<IMovable>();
        }


        protected override void Start()
        {
            Debug.LogWarning($"INPUT STARTED {_input != null}");
            
            _disposables.AddRange(new List<IDisposable>{
                    _input.Horizontal.AxisOnChange.Subscribe(OnHorizontalChanged),
                    _input.Vertical.AxisOnChange.Subscribe(OnVerticalChanged)}
                );
        }


        protected override void Update()
        {

        }

        protected override void FixedUpdate()
        {
            _direction.x = _horizontal;
            _direction.z = _vertical;
            _movable.Move(_direction);
        }


        protected override void OnDestroy()
        {
            _disposables.ForEach(d => d.Dispose());
        }


        private void OnHorizontalChanged(float value)
        {
            _horizontal = value;
            // Debug.Log($"HORIZONTAL CHANGED [{value}]");
            
        }


        private void OnVerticalChanged(float value)
        {
            
            _vertical = value;
            // Debug.Log($"VERTICAL CHANGED [{value}]");
            
        }


    }
}