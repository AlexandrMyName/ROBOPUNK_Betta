using System;
using Abstracts;
using UniRx;
using UnityEngine;

namespace Core
{
    internal sealed class PCInputHorizontal : IObservableInputProxy<float>
    {
        public IObservable<float> AxisOnChange { get; }

        public PCInputHorizontal(InputConfig config)
        {
            AxisOnChange = Observable
                .EveryUpdate()
                .Select(_ => config.Direction.Vector.ReadValue<Vector2>().x);
        }
    }
}