using Abstracts;
using UniRx;


namespace Core
{
    
    internal sealed class PCWeaponFirstInput : ISubjectInputProxy<Unit>
    {

        public Subject<Unit> AxisOnChange { get; } = new();

        
        public PCWeaponFirstInput(InputConfig config)
        {
            config.Weapon.First.performed += context => AxisOnChange.OnNext(Unit.Default);
        }
        
        
    }
}