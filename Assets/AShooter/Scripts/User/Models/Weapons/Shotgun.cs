using UnityEngine;


namespace User
{
    
    public class Shotgun : Weapon
    {

        public Shotgun(int weaponId, GameObject weaponObject, Projectile projectileObject, WeaponType weaponType,
            float damage, int clipSize, int leftPatronsCount, float reloadTime, float shootDistance, float shootSpeed,
            float fireSpread, LayerMask layerMask, ParticleSystem effect, float effectDestroyDelay) : base(
            weaponId, weaponObject, projectileObject, weaponType, damage, clipSize, leftPatronsCount,
            reloadTime, shootDistance, shootSpeed, fireSpread, layerMask, effect, effectDestroyDelay)
        {
            
        }
        
        
    }
}
