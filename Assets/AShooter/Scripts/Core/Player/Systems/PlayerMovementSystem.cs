using System;
using abstracts;
using UnityEngine;
using Zenject;


namespace Core 
{
    
    public sealed class PlayerMovementSystem : BaseSystem
    {

        [Inject] private IInput _input;
        private IGameComponents _components;
        private PlayerAnimator _animator;
        
        
        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _animator = components.BaseObject.GetComponent<PlayerAnimator>();
            Debug.Log($"Initialized move system! ({components.BaseObject.name})");
            if (_animator == null) Debug.LogWarning($"Player animator not found on {components.BaseObject.name}");
        }


        protected override void Start()
        {
            Debug.LogWarning($"INPUT STARTED {_input != null}");
            _input.Horizontal.AxisOnChange += OnHorizontalChanged;
        }
        
        
        protected override void Update()
        {
        
        }
        
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _input.Horizontal.AxisOnChange -= OnHorizontalChanged;
        }
        
        
        private void OnHorizontalChanged(float value)
        {
            Debug.Log($"HORIZONTAL CHANGED [{value}]");
        }


    }
}