using Abstracts;
using UnityEngine;


namespace User
{
    
    public class Shotgun : Weapon
    {

        private Transform _muzzle;
        
        
        
        public Shotgun(int weaponId, GameObject weaponObject, Projectile projectileObject, WeaponType weaponType,
            float damage, int clipSize, int leftPatronsCount, float reloadTime, float shootDistance, float shootSpeed,
            float fireSpread, float spreadFactor, LayerMask layerMask, 
            ParticleSystem effect, float effectDestroyDelay) : base(
            weaponId, weaponObject, projectileObject, weaponType, damage, clipSize, leftPatronsCount,
            reloadTime, shootDistance, shootSpeed, fireSpread, layerMask, effect, effectDestroyDelay)
        {
            _muzzle = FindMuzzle();
            SpreadFactor = spreadFactor;
        }


        public override void Shoot(Transform playerTransform, Camera camera, Vector3 mousePosition)
        {
            Debug.Log("SHOOT MTFCKR");
            LeftPatronsCount--;
            IsShootReady = false;
            ProcessShootTimeout();
            var hitPoint = FindHitPoint(camera, mousePosition);
            PerformAttack(hitPoint);
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


        private void PerformAttack(Vector3 hitPoint)
        {
            for (int i = 0; i < FireSpread; i++)
            {
                PerformRaycast(hitPoint);
            }
        }


        private void PerformRaycast(Vector3 hitPoint)
        {
            var ray = new Ray(_muzzle.position, hitPoint - _muzzle.position);
            var pointOnRayLimitedByDistance = ray.GetPoint(ShootDistance);
            var pointWithError = pointOnRayLimitedByDistance + CalculateSpread();

            ray = new Ray(_muzzle.position, pointWithError - _muzzle.position);

            Debug.DrawRay(ray.origin, ray.direction, Color.green, 10.0f);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, ShootDistance, LayerMask))
            {
                var hitCollider = hitInfo.collider;

                if (hitCollider.TryGetComponent(out IAttackable unit))
                {
                    Debug.Log($"Found target [{unit}] health {unit.Health}");
                    unit.TakeDamage(Damage / FireSpread);
                }
                else
                {
                    Debug.Log($"{hitCollider} IAttackable is not found");
                }

                SpawnParticleEffectOnHit(hitInfo);
            }
        }
        
        
        private void SpawnParticleEffectOnHit(RaycastHit hitInfo)
        {
            if (Effect != null)
            {
                var hitEffectRotation = Quaternion.LookRotation(hitInfo.normal);

                var hitEffect = GameObject.Instantiate(
                    Effect,
                    hitInfo.point,
                    hitEffectRotation);

                GameObject.Destroy(hitEffect.gameObject, EffectDestroyDelay);
            }
        }
        
        
        private Vector3 CalculateSpread()
        {
            return new Vector3
            {
                x = Random.Range(-SpreadFactor, SpreadFactor),
                y = Random.Range(-SpreadFactor, SpreadFactor),
                z = Random.Range(-SpreadFactor, SpreadFactor)
            };
        }
        
        
        
        
        
        
    }
}
