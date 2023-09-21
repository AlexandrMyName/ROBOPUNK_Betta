using UnityEngine;


namespace User
{
    
    public sealed class Sword : MeleeWeapon
    {
        
        public Sword(int weaponId, GameObject weaponObject, WeaponType weaponType, float damage, 
            LayerMask layerMask, ParticleSystem effect, float effectDestroyDelay, float attackTimeout) 
            : base(weaponId, weaponObject, weaponType, damage, layerMask, effect, effectDestroyDelay, attackTimeout)
        {
            
        }
        
        
    }
}