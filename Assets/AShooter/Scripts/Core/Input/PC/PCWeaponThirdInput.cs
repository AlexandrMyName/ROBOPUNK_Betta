using Abstracts;
using UniRx;


namespace Core
{
    
    internal sealed class PCWeaponThirdInput : ISubjectInputProxy<Unit>
    {

        public Subject<Unit> AxisOnChange { get; } = new();

        
        public PCWeaponThirdInput(InputConfig config)
        {
            config.Weapon.Third.performed += context => AxisOnChange.OnNext(Unit.Default);
        }
        
        
    }
}