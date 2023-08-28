using System;


namespace Abstracts
{
    
    public interface IUserInputProxy<T>
    {

        public IObservable<T> AxisOnChange { get; }
        
    }
}