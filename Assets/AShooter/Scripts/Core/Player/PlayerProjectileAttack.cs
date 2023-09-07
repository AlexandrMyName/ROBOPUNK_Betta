using UnityEngine;
using User;
using Zenject;


namespace Core
{

    public sealed class PlayerProjectileAttack
    {

        private Camera _camera;
        private Weapon _weapon;
        private Transform _weaponMuzzle;
        private Vector3 _mousePosition;
        private ForceMode _forceMode = ForceMode.Impulse;
        private float _force = 10f;


        public PlayerProjectileAttack(Weapon weapon, Transform weaponMuzzle, Camera camera, Vector3 mousePosition)
        {
            _camera = camera;
            _weapon = weapon;
            _weaponMuzzle = weaponMuzzle;
            _mousePosition = mousePosition;
        }


        public PlayerProjectileAttack(Weapon weapon, Transform weaponMuzzle, Camera camera, Vector3 mousePosition, ForceMode forceMode, float force)
        {
            _camera = camera;
            _weapon = weapon;
            _weaponMuzzle = weaponMuzzle;
            _mousePosition = mousePosition;
            _forceMode = forceMode;
            _force = force;
        }


        public void Attack()
        {
            InstantiateProjectile();
        }


        public void InstantiateProjectile()
        {
            var projectile = GameObject.Instantiate(_weapon.ProjectileObject, _weaponMuzzle.position, _weaponMuzzle.rotation);
            projectile.Damage = _weapon.Damage;
            projectile.Effect = _weapon.Effect;
            projectile.EffectDestroyDelay = _weapon.EffectDestroyDelay;
            projectile.Rigidbody.AddForce(_weaponMuzzle.forward * _force, _forceMode);
        }


    }
}
