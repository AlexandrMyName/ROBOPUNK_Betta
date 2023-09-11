using System;
using UniRx;


namespace Abstracts
{
    
    public interface ISubjectInputProxy<T>
    {

        public Subject<T> AxisOnChange { get; }
        
    }
}