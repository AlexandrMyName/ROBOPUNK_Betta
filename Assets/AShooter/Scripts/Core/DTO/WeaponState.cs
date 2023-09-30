using Abstracts;
using UniRx;


namespace Core.DTO
{
    
    public class WeaponState
    {

        public ReactiveProperty<IWeapon> CurrentWeapon { get; }

        public ReactiveProperty<IWeapon> MainWeapon { get; }

        public ReactiveProperty<IWeapon> PickUpWeapon { get; }

        public ReactiveProperty<bool> IsMeleeWeaponPressed { get; }


        public WeaponState()
        {
            CurrentWeapon = new ReactiveProperty<IWeapon>();
            MainWeapon = new ReactiveProperty<IWeapon>();
            PickUpWeapon = new ReactiveProperty<IWeapon>();
            IsMeleeWeaponPressed = new ReactiveProperty<bool>();
        }
        

    }
}