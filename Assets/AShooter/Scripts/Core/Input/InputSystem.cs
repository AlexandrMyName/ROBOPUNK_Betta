using System;
using abstracts;
using UniRx;
using UnityEngine;
using Zenject;


namespace Core
{
    
    public sealed class InputSystem : BaseSystem
    {

        [Inject] private IInput _input;


        protected override void Start()
        {
            base.Start();
            // _input.Horizontal = Observable.EveryUpdate().Select(_ => Input.GetAxis("Horizontal"));
        }


        protected override void Update()
        {
            _input.Horizontal.GetAxis();
            _input.Vertical.GetAxis();
            _input.LeftClick.GetAxis();
        }
        
        
        
    }
}