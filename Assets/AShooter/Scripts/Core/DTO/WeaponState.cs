using Abstracts;
using UniRx;


namespace Core.DTO
{
    
    public class WeaponState
    {

        public ReactiveProperty<IWeapon> CurrentWeapon { get; }

        public ReactiveProperty<bool> IsMeleeWeaponPressed { get; }


        public WeaponState()
        {
            CurrentWeapon = new ReactiveProperty<IWeapon>();
            IsMeleeWeaponPressed = new ReactiveProperty<bool>();
        }
        

    }
}