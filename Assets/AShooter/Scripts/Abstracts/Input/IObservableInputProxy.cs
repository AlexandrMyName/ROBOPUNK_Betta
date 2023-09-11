using System;


namespace Abstracts
{
    
    public interface IObservableInputProxy<T>
    {

        public IObservable<T> AxisOnChange { get; }
        
    }
}