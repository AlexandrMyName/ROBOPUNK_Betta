using System.Linq;
using Abstracts;
using UnityEngine;


namespace Core
{

    public sealed class RaycastAttack : IRayAttack
    {

        private Camera _camera;
        private IRangeWeapon _weapon;
        private Transform _muzzle;

        private RaycastHit _cameraHit;
        
        private Vector3 _hitPointFromMuzzle;
        private Vector3 _cameraHitPoint;
        

        public RaycastAttack(IRangeWeapon weapon, Camera camera)
        {
            _camera = camera;
            _weapon = weapon;

            _muzzle = FindMuzzle(_weapon);
        }


        public void Attack(Vector3 mousePosition)
        {
            if (FindCameraHitPoint(mousePosition))
            {
                for (int i = 0; i < _weapon.FireSpread; i++)
                {
                    PerformRayAttack(_cameraHit);
                }
            }
            
            SpawnParticleEffectOnMuzzle();
        }


        private void PerformRayAttack(RaycastHit cameraHitPoint)
        {
            
            var muzzleDirection = (cameraHitPoint.point - _muzzle.position).normalized;
            var muzzleRay = new Ray(_muzzle.position, muzzleDirection);
            var pointOnRayLimitedByDistance = muzzleRay.GetPoint(_weapon.ShootDistance);
            
            var pointWithError = _weapon.FireSpread <= 1 ? 
                pointOnRayLimitedByDistance : 
                pointOnRayLimitedByDistance + CalculateSpread();

            muzzleDirection = (pointWithError - _muzzle.position).normalized;
            muzzleRay = new Ray(_muzzle.position, muzzleDirection);
            
            RaycastHit[] muzzleHits = Physics.RaycastAll(muzzleRay, _weapon.ShootDistance, _weapon.LayerMask);

            bool isMuzzleHit = muzzleHits.Select(h => !h.collider.isTrigger).FirstOrDefault();

            if (isMuzzleHit)
            {
                if (Physics.Raycast(muzzleRay, out var muzzleHit, _weapon.ShootDistance, _weapon.LayerMask))
                {
                    // RaycastHit muzzleHit = muzzleHits.LastOrDefault(h => !h.collider.isTrigger);
                    Debug.DrawRay(_muzzle.position, muzzleDirection * _weapon.ShootDistance, Color.green, 20.0f);
                    var hitCollider = muzzleHit.collider;

                    if (hitCollider.TryGetComponent(out IEnemy unit))
                    {
                        unit.ComponentsStore.Attackable.TakeDamage(_weapon.Damage);
                    }
                    SpawnParticleEffectOnHit(muzzleHit);
                }
            }
        }


        private bool FindCameraHitPoint(Vector3 mousePosition)
        {
            bool isCameraHit = false;
            var cameraRay = _camera.ScreenPointToRay(mousePosition);

            RaycastHit[] cameraHits = Physics.RaycastAll(cameraRay, Mathf.Infinity, _weapon.LayerMask);
            isCameraHit = cameraHits.Select(h => !h.collider.isTrigger).FirstOrDefault();

            if (isCameraHit)
            {
                _cameraHit = cameraHits.FirstOrDefault(h => !h.collider.isTrigger);
            }

            return isCameraHit;
        }


        private void SpawnParticleEffectOnHit(RaycastHit hitInfo)
        {
            if (_weapon.Effect != null)
            {
                var hitEffectRotation = Quaternion.LookRotation(hitInfo.normal);

                var hitEffect = GameObject.Instantiate(
                    _weapon.Effect,
                    hitInfo.point,
                    hitEffectRotation,
                    hitInfo.transform.TryGetComponent(out IEnemy enemy) ? hitInfo.transform : null);

                GameObject.Destroy(hitEffect.gameObject, _weapon.EffectDestroyDelay);
            }
        }


        private void SpawnParticleEffectOnMuzzle()
        {
            if (_weapon.MuzzleEffect != null)
            {
                var muzzle = FindMuzzle(_weapon);

                if (muzzle)
                {
                    var muzzleEffect = GameObject.Instantiate(
                        _weapon.MuzzleEffect,
                        muzzle);

                    GameObject.Destroy(muzzleEffect.gameObject, _weapon.EffectDestroyDelay);
                }
            }
        }


        private Transform FindMuzzle(IWeapon weapon)
        {
            Transform muzzle = null;
            
            foreach (Transform child in weapon.WeaponObject.transform)
            {
                if (child.CompareTag("Muzzle"))
                {
                    muzzle = child;
                    break;
                }
            }
            return muzzle;
        }
        
        
        private Vector3 CalculateSpread()
        {
            return new Vector3
            {
                x = Random.Range(-_weapon.SpreadFactor, _weapon.SpreadFactor),
                y = Random.Range(-_weapon.SpreadFactor, _weapon.SpreadFactor),
                z = Random.Range(-_weapon.SpreadFactor, _weapon.SpreadFactor)
            };
        }

        
    }
}
