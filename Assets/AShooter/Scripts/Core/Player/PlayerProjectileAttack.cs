using UnityEngine;
using User;
using Zenject;


namespace Core
{

    public sealed class PlayerProjectileAttack
    {

        private Weapon _weapon;
        private Transform _weaponMuzzle;
        private ForceMode _forceMode = ForceMode.Impulse;
        private float _force = 20f;
        

        public PlayerProjectileAttack(Weapon weapon, Transform weaponMuzzle)
        {
            _weapon = weapon;
            _weaponMuzzle = weaponMuzzle;
        }


        public PlayerProjectileAttack(Weapon weapon, Transform weaponMuzzle, ForceMode forceMode, float force)
        {
            _weapon = weapon;
            _weaponMuzzle = weaponMuzzle;
            _forceMode = forceMode;
            _force = force;
        }


        public void Attack()
        {
            var projectile = GameObject.Instantiate(_weapon.ProjectileObject, _weaponMuzzle.position, _weaponMuzzle.rotation);
            projectile.Damage = _weapon.Damage;
            projectile.Effect = _weapon.Effect;
            projectile.EffectDestroyDelay = _weapon.EffectDestroyDelay;
            projectile.Rigidbody.AddForce(_weaponMuzzle.forward * _force, _forceMode);
        }


    }
}
