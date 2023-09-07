using Abstracts;
using Core;
using UnityEngine;


namespace User
{

    public abstract class Weapon : IWeapon
    {

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


        public virtual void Shoot(Camera camera, Vector3 mousePosition)
        {
            PlayerSimpleAttack simpleAttack = new PlayerSimpleAttack(this, camera, mousePosition);
            simpleAttack.Attack();
        }


    }
}
