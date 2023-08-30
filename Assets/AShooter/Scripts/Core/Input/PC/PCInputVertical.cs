using System;
using Abstracts;
using UniRx;
using UnityEngine;

namespace Core
{
    
    internal sealed class PCInputVertical : IUserInputProxy<float>
    {
        public IObservable<float> AxisOnChange { get; }

        public PCInputVertical(InputConfig _cnf)
        { 
            AxisOnChange = Observable
                .EveryUpdate()
                .Select(_ => _cnf.Direction.Vector.ReadValue<Vector2>().y);
        }  
    }
}