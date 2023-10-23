using UniRx;
using UnityEngine;


namespace Abstracts
{
    
    public interface IInput
    {
        
        IObservableInputProxy<float> Horizontal { get; }
        
        IObservableInputProxy<float> Vertical { get; }

        IObservableInputProxy<bool> LeftClick { get; }
        
        IObservableInputProxy<bool> RightClick { get; }
        
        ISubjectInputProxy<Unit> DashClick { get; }

        IObservableInputProxy<Vector3> MousePosition { get; }

        ISubjectInputProxy<Unit> WeaponFirst { get; }
        
        ISubjectInputProxy<Unit> WeaponSecond { get; }
        
        ISubjectInputProxy<Unit> WeaponThird { get; }

        ISubjectInputProxy<Unit> Explosion { get; }
        
        ISubjectInputProxy<Unit> Interact { get; }

        IObservableInputProxy<bool> MeleeHold { get; }

        ISubjectInputProxy<Unit> PauseMenu { get; }

        ISubjectInputProxy<Unit> MP3Player { get; }

    }
}