using System;
using Abstracts;
using UniRx;


namespace Core
{

    internal sealed class PCLMBInput : IObservableInputProxy<bool>
    {

        public IObservable<bool> AxisOnChange { get; }


        public PCLMBInput(InputConfig config)
        {
            AxisOnChange = Observable
                .EveryUpdate()
                .Select(_ => config.Mouse.LBM.IsPressed());
        }
        
        
    }
}