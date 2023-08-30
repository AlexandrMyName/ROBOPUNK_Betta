using System;
using Abstracts;
using UniRx;
using UnityEngine;

namespace Core
{
    internal sealed class PCInputHorizontal : IUserInputProxy<float>
    {
        public IObservable<float> AxisOnChange { get; }

        public PCInputHorizontal(InputConfig _cnf)
        {
            AxisOnChange = Observable
                .EveryUpdate()
                .Select(_ => _cnf.Direction.Vector.ReadValue<Vector2>().x);
        }
    }
}