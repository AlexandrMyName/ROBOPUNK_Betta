using Abstracts;
using Unity.VisualScripting;
using UnityEngine;
using User;


namespace Core
{

    public sealed class RaycastAttack
    {

        private Camera _camera;
        private IRangeWeapon _weapon;
        private Vector3 _mousePosition;
        private Transform _playerTransform;


        public RaycastAttack(IRangeWeapon weapon, Transform playerTransform, Camera camera, Vector3 mousePosition)
        {
            _camera = camera;
            _playerTransform = playerTransform;
            _weapon = weapon;
            _mousePosition = mousePosition;
        }


        public void Attack()
        {
            FindTarget();
            SpawnParticleEffectOnMuzzle();
        }


        private void FindTarget()
        {
            var ray = _camera.ScreenPointToRay(_mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _weapon.LayerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.green);

                var hitCollider = hitInfo.collider;

                if (hitCollider.TryGetComponent(out IEnemy unit))
                {
                    Debug.Log($"Found target [{unit}] health {unit.ComponentsStore.Attackable.Health}");
                    unit.ComponentsStore.Attackable.TakeDamage(_weapon.Damage);
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


        public Transform FindMuzzle(IWeapon weapon)
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
