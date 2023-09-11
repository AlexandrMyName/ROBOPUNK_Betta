using System;
using Abstracts;
using UniRx;
using UnityEngine;


namespace Core
{
    
    internal sealed class PCMousePositionInput : IObservableInputProxy<Vector3>
    {
        
        public IObservable<Vector3> AxisOnChange { get; }

        public PCMousePositionInput()
        {
            AxisOnChange = Observable
                .EveryUpdate()
                .Select(_ => Input.mousePosition);
        }
        
        
    }
}