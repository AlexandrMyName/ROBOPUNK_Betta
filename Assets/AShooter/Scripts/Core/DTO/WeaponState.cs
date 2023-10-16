using Abstracts;
using UniRx;


namespace Core.DTO
{
    
    public class WeaponState
    {

        public ReactiveProperty<IMeleeWeapon> MeleeWeapon { get; }

        public ReactiveProperty<IRangeWeapon> MainWeapon { get; }

        public ReactiveProperty<IRangeWeapon> SecondaryWeapon { get; }

        public ReactiveProperty<bool> IsMeleeWeaponPressed { get; }

        public IWeapon CurrentWeapon { get; set; }


        public WeaponState()
        {
            MeleeWeapon = new ReactiveProperty<IMeleeWeapon>();
            MainWeapon = new ReactiveProperty<IRangeWeapon>();
            SecondaryWeapon = new ReactiveProperty<IRangeWeapon>();
            IsMeleeWeaponPressed = new ReactiveProperty<bool>();
        }
        

    }
}