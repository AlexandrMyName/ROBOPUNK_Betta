using System;
using Abstracts;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Core
{
    
    internal sealed class PCMousePositionInput : IObservableInputProxy<Vector3>
    {
        
        public IObservable<Vector3> AxisOnChange { get; }
        
        
        public PCMousePositionInput()
        {
            AxisOnChange = Observable
                .EveryUpdate()
                .Select(_ => (Vector3) Mouse.current.position.ReadValue());
        }
        
        
    }
}