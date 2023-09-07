using Abstracts;
using UniRx;


namespace Core.DTO
{
    
    public class WeaponState
    {

        public ReactiveProperty<IWeapon> CurrentWeapon { get; }

        public WeaponState()
        {
            CurrentWeapon = new ReactiveProperty<IWeapon>();
        }
        

    }
}