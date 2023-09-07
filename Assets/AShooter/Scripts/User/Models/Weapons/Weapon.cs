using Abstracts;
using Core;
using System.Collections;
using System.Threading;
using Tool;
using UnityEngine;


namespace User
{

    public abstract class Weapon : IWeapon
    {

        private Coroutine _reloadCoroutine;


        public int WeaponId { get; protected set; }

        public GameObject WeaponObject { get; protected set; }

        public Projectile ProjectileObject { get; protected set; }

        public WeaponType WeaponType { get; protected set; }
        
        public float Damage { get; protected set; }

        public int ClipSize { get; protected set; }
        
        public int LeftPatronsCount { get; protected set; }
        
        public float ReloadTime { get; protected set; }
        
        public float ShootDistance { get; protected set; }
        
        public float ShootSpeed { get; protected set; }
        
        public float FireSpread { get; protected set; }

        public LayerMask LayerMask { get; protected set; }

        public ParticleSystem Effect { get; protected set; }

        public float EffectDestroyDelay { get; protected set; }


        public virtual void Shoot(Transform playerTransform, Camera camera, Vector3 mousePosition)
        {
            if (LeftPatronsCount <= 0)
                Reload();
            else
            {
                if (_reloadCoroutine != null)
                {
                    Coroutines.StoptRoutine(_reloadCoroutine);
                    _reloadCoroutine = null;
                }

                PlayerSimpleAttack simpleAttack = new PlayerSimpleAttack(this, playerTransform, camera, mousePosition);
                simpleAttack.Attack();
            }

            LeftPatronsCount--;
        }


        public void Reload()
        {
            if (_reloadCoroutine != null)
                return;

            _reloadCoroutine = Coroutines.StartRoutine(ReloadCoroutine());
        }


        private IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(ReloadTime);
            LeftPatronsCount = ClipSize;
        }


    }
}
