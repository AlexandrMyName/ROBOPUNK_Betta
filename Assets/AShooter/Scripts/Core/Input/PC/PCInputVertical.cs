using System;
using Abstracts;
using UniRx;
using UnityEngine;

namespace Core
{
    
    internal sealed class PCInputVertical : IObservableInputProxy<float>
    {
        public IObservable<float> AxisOnChange { get; }

        public PCInputVertical(InputConfig config)
        { 
            AxisOnChange = Observable
                .EveryUpdate()
                .Select(_ => config.Direction.Vector.ReadValue<Vector2>().y);
        }  
    }
}