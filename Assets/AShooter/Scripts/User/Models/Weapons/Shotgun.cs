using UnityEngine;


namespace User
{
    
    public class Shotgun : Weapon
    {

        private Transform _muzzle;
        
        

        public Shotgun(int weaponId, GameObject weaponObject, Projectile projectileObject, WeaponType weaponType,
            float damage, int clipSize, int leftPatronsCount, float reloadTime, float shootDistance, float shootSpeed,
            float fireSpread, LayerMask layerMask, ParticleSystem effect, float effectDestroyDelay) : base(
            weaponId, weaponObject, projectileObject, weaponType, damage, clipSize, leftPatronsCount,
            reloadTime, shootDistance, shootSpeed, fireSpread, layerMask, effect, effectDestroyDelay)
        {
            _muzzle = FindMuzzle();
        }


        public override void Shoot(Transform playerTransform, Camera camera, Vector3 mousePosition)
        {
            
        }
        
        
        private Transform FindMuzzle()
        {
            Transform muzzle = null;
            
            foreach (Transform child in WeaponObject.transform)
            {
                if (child.CompareTag("Muzzle"))
                {
                    muzzle = child;
                    break;
                }
            }
            return muzzle;
        }
        
        
        private Vector3 FindHitPoint(Camera _camera, Vector3 mousePosition)
        {
            var hitPoint = Vector3.zero;
            
            var ray = _camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.green);

                var hitCollider = hitInfo.collider;
                hitPoint = hitInfo.point;
            }
            
            return hitPoint;
        }
        
        
    }
}
