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
        private Rigidbody _rigidbody;
        
        private List<IDisposable> _disposables = new();


        protected override void Awake(IGameComponents components)
        {
            _rigidbody = _components.BaseObject.GetComponent<Rigidbody>();
            _components = components;
            _animator = components.BaseObject.GetComponent<PlayerAnimator>();
            Debug.Log($"Initialized move system! ({components.BaseObject.name})");
            if (_animator == null) Debug.LogWarning($"Player animator not found on {components.BaseObject.name}");
        }


        protected override void Start()
        {
            Debug.LogWarning($"INPUT STARTED {_input != null}");
            
            _disposables.AddRange(new List<IDisposable>{
                    _input.Horizontal.AxisOnChange.Subscribe(OnHorizontalChanged),
                    _input.Vertical.AxisOnChange.Subscribe(OnVerticalChanged),
                    _input.LeftClick.AxisOnChange.Subscribe(OnLeftClick),
                    _input.MousePosition.AxisOnChange.Subscribe(OnMousePositionChanged)}
                );
        }


        protected override void Update()
        {

        }


        protected override void OnDestroy()
        {
            _disposables.ForEach(d => d.Dispose());
        }


        private void OnHorizontalChanged(float value)
        {
            // Debug.Log($"HORIZONTAL CHANGED [{value}]");
            Move();
        }


        private void OnVerticalChanged(float value)
        {
            // Debug.Log($"VERTICAL CHANGED [{value}]");
            Move();
        }


        private void OnLeftClick(bool isClicked)
        {
            if (isClicked) 
                Debug.Log($"LMB CHANGED [{isClicked}]");
        }


        private void OnMousePositionChanged(Vector3 position)
        {
            // Debug.Log($"MOUSE POSITION CHANGED [{position}]");
            Move();
        }

        private void Move()
        {
            var Horizontal = Input.GetAxis(AxisManager.HORIZONTAL);
            var Vertical = Input.GetAxis(AxisManager.VERTICAL);
            _rigidbody.velocity = new Vector3(Horizontal*100,_rigidbody.velocity.y*100,Vertical*100);
            
        }
    }
}