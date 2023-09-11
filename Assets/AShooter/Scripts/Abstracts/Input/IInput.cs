using UniRx;
using UnityEngine;


namespace Abstracts
{
    
    public interface IInput
    {
        
        IObservableInputProxy<float> Horizontal { get; }
        
        IObservableInputProxy<float> Vertical { get; }

        IObservableInputProxy<bool> LeftClick { get; }

        
        IObservableInputProxy<Vector3> MousePosition { get; }

        ISubjectInputProxy<Unit> WeaponFirst { get; }
        
        ISubjectInputProxy<Unit> WeaponSecond { get; }
        
        ISubjectInputProxy<Unit> WeaponThird { get; }


    }
}