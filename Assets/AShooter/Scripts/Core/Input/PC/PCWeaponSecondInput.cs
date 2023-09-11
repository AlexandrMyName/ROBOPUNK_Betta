using Abstracts;
using UniRx;


namespace Core
{
    
    internal sealed class PCWeaponSecondInput : ISubjectInputProxy<Unit>
    {

        public Subject<Unit> AxisOnChange { get; } = new();

        
        public PCWeaponSecondInput(InputConfig config)
        {
            config.Weapon.Second.performed += context => AxisOnChange.OnNext(Unit.Default);
        } 
        
        
    }
}