using Abstracts;
using UniRx;


namespace Core.DTO
{
    
    public class WeaponState
    {
        
        public ReactiveProperty<IWeapon> CurrentWeapon = new();
        
    }
}