using System;
using Abstracts;
using Core;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


namespace User
{

    public abstract class Weapon : IWeapon, IDisposable
    {
        
        public int WeaponId { get; protected set; }

        public GameObject WeaponObject { get; protected set; }

        public Projectile ProjectileObject { get; protected set; }
        
        public float ProjectileForce { get; protected set; }
        
        public WeaponType WeaponType { get; protected set; }
        
        public float Damage { get; protected set; }

        public int ClipSize { get; protected set; }
        
        public int LeftPatronsCount { get; protected set; }
        
        public float ReloadTime { get; protected set; }
        
        public float ShootDistance { get; protected set; }
        
        public float ShootSpeed { get; protected set; }
        
        public float FireSpread { get; protected set; }
        
        public float SpreadFactor { get; protected set; }

        public LayerMask LayerMask { get; protected set; }

        public ParticleSystem Effect { get; protected set; }

        public float EffectDestroyDelay { get; protected set; }


        private List<IDisposable> _disposables = new();

        
        public bool IsReloadProcessing { get; protected set; }

        public bool IsShootReady { get; protected set; }


        public Weapon(int weaponId, GameObject weaponObject, Projectile projectileObject, 
            WeaponType weaponType, float damage, int clipSize, int leftPatronsCount,
            float reloadTime, float shootDistance, float shootSpeed, float fireSpread, 
            LayerMask layerMask, ParticleSystem effect, float effectDestroyDelay)
        {
            WeaponId = weaponId;
            WeaponObject = weaponObject;
            ProjectileObject = projectileObject;
            WeaponType = weaponType;
            Damage = damage;
            ClipSize = clipSize;
            LeftPatronsCount = leftPatronsCount;
            ReloadTime = reloadTime;
            ShootDistance = shootDistance;
            ShootSpeed = shootSpeed;
            FireSpread = fireSpread;
            LayerMask = layerMask;
            Effect = effect;
            EffectDestroyDelay = effectDestroyDelay;
            IsShootReady = true;
        }
        

        public virtual void Shoot(Transform playerTransform, Camera camera, Vector3 mousePosition)
        {
            RaycastAttack simpleAttack = new RaycastAttack(this, playerTransform, camera, mousePosition);
            simpleAttack.Attack();

            LeftPatronsCount--;
            IsShootReady = false;
            ProcessShootTimeout();

            Debug.Log("SHOOT MTFCKR");
        }


        public void Reload()
        {
            LeftPatronsCount = ClipSize;
            Debug.Log("RELOAD MTFCKR");
            IsReloadProcessing = false;
        }


        public void ProcessReload()
        {
            if (!IsReloadProcessing)
            {
                IsReloadProcessing = true;
                
                _disposables.Add(
                    Observable
                        .Timer(TimeSpan.FromSeconds(ReloadTime))
                        .Subscribe(_ => Reload())
                );
            }
        }


        public void ProcessShootTimeout()
        {
            if (!IsShootReady)
            {
                _disposables.Add(
                    Observable
                        .Timer(TimeSpan.FromSeconds(ShootSpeed))
                        .Subscribe(_ => IsShootReady = true)
                );
            }
        }


        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
        }
        
        
    }
}
