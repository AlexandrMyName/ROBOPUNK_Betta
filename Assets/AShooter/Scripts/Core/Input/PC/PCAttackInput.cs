using System;
using Abstracts;
using UniRx;
using UnityEngine;


namespace Core
{

    internal sealed class PCAttackInput : IObservableInputProxy<bool>
    {
        
        public IObservable<bool> AxisOnChange { get; }


        public PCAttackInput()
        {
            AxisOnChange = Observable
                .EveryUpdate()
                .Select(_ => Input.GetButtonDown(AxisManager.FIRE1));
        }
        
    }
}