using System;
using Abstracts;
using UniRx;
using UnityEngine;


namespace Core
{
    
    internal sealed class PCInputVertical : IUserInputProxy<float>
    {
        
        public IObservable<float> AxisOnChange { get; }


        public PCInputVertical()
        {
            AxisOnChange = Observable
                .EveryUpdate()
                .Select(_ => Input.GetAxis(AxisManager.VERTICAL));
        }
        
            
    }
}