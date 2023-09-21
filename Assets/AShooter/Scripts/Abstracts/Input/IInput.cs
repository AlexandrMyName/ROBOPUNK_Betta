using UniRx;
using UnityEngine;


namespace Abstracts
{
    
    public interface IInput
    {
        
        IObservableInputProxy<float> Horizontal { get; }
        
        IObservableInputProxy<float> Vertical { get; }

        ISubjectInputProxy<Unit> LeftClick { get; }
        
        ISubjectInputProxy<Unit> RightClick { get; }
        
        ISubjectInputProxy<Unit> DashClick { get; }

        IObservableInputProxy<Vector3> MousePosition { get; }

        ISubjectInputProxy<Unit> WeaponFirst { get; }
        
        ISubjectInputProxy<Unit> WeaponSecond { get; }
        
        ISubjectInputProxy<Unit> WeaponThird { get; }

        ISubjectInputProxy<Unit> Explosion { get; }

        IObservableInputProxy<bool> MeleeHold { get; }


    }
}