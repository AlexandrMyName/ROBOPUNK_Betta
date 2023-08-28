using System;
using Abstracts;
using UniRx;
using UnityEngine;


namespace Core
{
    
    internal sealed class PCInputHorizontal : IUserInputProxy<float>
    {
        
        public IObservable<float> AxisOnChange { get; }

        
        public PCInputHorizontal()
        {
            AxisOnChange = Observable
                .EveryUpdate()
                .Select(_ => Input.GetAxis(AxisManager.HORIZONTAL));
        }
        
        
    }
}