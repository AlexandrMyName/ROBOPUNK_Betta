using Abstracts;
using UniRx;


namespace Core.DTO
{
    
    public class WeaponState
    {

        public ReactiveProperty<IWeapon> CurrentWeapon { get; }
        public ReactiveProperty<int> LeftPatrons { get; }

        public bool IsNeedReload;
        
        
        public WeaponState()
        {
            CurrentWeapon = new ReactiveProperty<IWeapon>();
            LeftPatrons = new ReactiveProperty<int>();
        }
        

    }
}