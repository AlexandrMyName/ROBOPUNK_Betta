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


        private Vector3 CalculateSpread()
        {
            float _spreadFactor = _weapon.FireSpread;

            return new Vector3
            {
                x = Random.Range(-_spreadFactor, _spreadFactor),
                y = Random.Range(-_spreadFactor, _spreadFactor),
                z = Random.Range(-_spreadFactor, _spreadFactor)
            };
        }


        private void SpawnParticleEffectOnHit(RaycastHit hitInfo)
        {
            if (_weapon.Effect != null)
            {
                var hitEffectRotation = Quaternion.LookRotation(hitInfo.normal);

                var hitEffect = GameObject.Instantiate(
                    _weapon.Effect,
                    hitInfo.point,
                    hitEffectRotation);

                GameObject.Destroy(hitEffect.gameObject, _weapon.EffectDestroyDelay);
            }
        }


    }
}
