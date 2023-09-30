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
        
        private Vector3 _mousePosition;
        private Vector3 _hitPointFromCamera;
        private Vector3 _hitPointFromMuzzle;


        public RaycastAttack(IRangeWeapon weapon, Camera camera)
        {
            _camera = camera;
            _weapon = weapon;

            _muzzle = FindMuzzle(_weapon);
        }


        public void Attack(Vector3 mousePosition)
        {
            _mousePosition = mousePosition;
            FindTarget();
            SpawnParticleEffectOnMuzzle();
        }


        private void FindTarget()
        {
            var cameraRay = _camera.ScreenPointToRay(_mousePosition);

            RaycastHit[] cameraHits = Physics.RaycastAll(cameraRay, Mathf.Infinity, _weapon.LayerMask);
            bool isCameraHit = cameraHits.Select(h => !h.collider.isTrigger).FirstOrDefault();
            
            if (isCameraHit)
            {
                RaycastHit cameraHit = cameraHits.FirstOrDefault(h => !h.collider.isTrigger);
                _hitPointFromCamera = cameraHit.point;

                var muzzleRay = new Ray(_muzzle.position, _hitPointFromCamera);
                RaycastHit[] muzzleHits = Physics.RaycastAll(muzzleRay, _weapon.ShootDistance, _weapon.LayerMask);

                bool isMuzzleHit = muzzleHits.Select(h => !h.collider.isTrigger).FirstOrDefault();

                if (isMuzzleHit)
                {
                    RaycastHit muzzleHit = muzzleHits.FirstOrDefault(h => !h.collider.isTrigger);
                    var hitCollider = muzzleHit.collider;
                    
                    if (hitCollider.TryGetComponent(out IEnemy unit))
                    {
                        unit.ComponentsStore.Attackable.TakeDamage(_weapon.Damage);
                    }
                    SpawnParticleEffectOnHit(muzzleHit);
                }
            }
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

        
    }
}
