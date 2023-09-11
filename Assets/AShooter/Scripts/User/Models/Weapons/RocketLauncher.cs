using Core;
using UnityEngine;


namespace User
{
    
    public class RocketLauncher : Weapon
    {

        private ProjectileAttack _projectileAttack;
        private Transform _muzzle;
        
        
        public RocketLauncher(int weaponId, GameObject weaponObject, Projectile projectileObject, float projectileForce, 
            WeaponType weaponType, float damage, int clipSize, int leftPatronsCount, float reloadTime, 
            float shootDistance, float shootSpeed, float fireSpread, LayerMask layerMask, ParticleSystem effect, 
            float effectDestroyDelay) : base(
            weaponId, weaponObject, projectileObject, weaponType, damage, clipSize, leftPatronsCount,
            reloadTime, shootDistance, shootSpeed, fireSpread, layerMask, effect, effectDestroyDelay)
        {
            ProjectileForce = projectileForce;
            _muzzle = FindMuzzle();
        }


        public override void Shoot(Transform playerTransform, Camera camera, Vector3 mousePosition)
        {
            Debug.Log("SHOOT MTFCKR");
            LeftPatronsCount--;
            var hitPoint = FindHitPoint(camera, mousePosition);
            InstantiateProjectile(hitPoint);
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
        
        
        private void InstantiateProjectile(Vector3 hitPoint)
        {
            var projectile = GameObject.Instantiate(ProjectileObject, _muzzle.position, _muzzle.rotation);
            projectile.Damage = Damage;
            projectile.Effect = Effect;
            projectile.EffectDestroyDelay = EffectDestroyDelay;
            var direction = (hitPoint - _muzzle.position).normalized;

            projectile.Rigidbody.AddForce(direction * ProjectileForce, ForceMode.Impulse);
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
