using System;
using Abstracts;
using UniRx;
using UnityEngine;


namespace Core
{
    
    internal sealed class PCMeleeInput : IObservableInputProxy<bool>
    {

        public IObservable<bool> AxisOnChange { get; }

        
        public PCMeleeInput(InputConfig config)
        {
            AxisOnChange = Observable
                .EveryUpdate()
                .Select(_ => config.Weapon.Melee.IsInProgress());
        }
        
        
    }
}