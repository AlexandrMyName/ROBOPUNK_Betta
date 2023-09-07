using Abstracts;
using UnityEngine;
using User;


namespace Core
{

    public sealed class PlayerSimpleAttack
    {

        private Camera _camera;
        private IWeapon _weapon;
        private Vector3 _mousePosition;


        public PlayerSimpleAttack(IWeapon weapon, Camera camera, Vector3 mousePosition)
        {
            _camera = camera;
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
                var hitCollider = hitInfo.collider;

                if (hitCollider.TryGetComponent(out IAttackable unit))
                {
                    Debug.Log($"Found target [{unit}]");
                    unit.TakeDamage(_weapon.Damage);
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
                    hitEffectRotation);

                GameObject.Destroy(hitEffect.gameObject, _weapon.EffectDestroyDelay);
            }
        }


    }
}
