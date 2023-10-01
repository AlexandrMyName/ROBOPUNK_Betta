using System;
using Abstracts;
using UniRx;


namespace Core
{

    internal sealed class PCRMBInput : IObservableInputProxy<bool>
    {

        public IObservable<bool> AxisOnChange { get; }


        public PCRMBInput(InputConfig config)
        {
            AxisOnChange = Observable
                .EveryUpdate()
                .Select(_ => config.Mouse.RBM.IsPressed());
        }
         
        
    }
}