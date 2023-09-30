using System;
using Abstracts;
using Core;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


namespace User
{

    public abstract class Weapon : IRangeWeapon, IDisposable
    {
        
        public int WeaponId { get; protected set; }

        public GameObject WeaponObject { get; protected set; }

        public Sprite WeaponIcon { get; protected set; }

        public Projectile ProjectileObject { get; protected set; }

        public float ProjectileForce { get; protected set; }
        
        public WeaponType WeaponType { get; protected set; }
        
        public float Damage { get; protected set; }

        public int ClipSize { get; protected set; }

        public ReactiveProperty<int> LeftPatronsCount { get; protected set; }

        public float ReloadTime { get; protected set; }
        
        public float ShootDistance { get; protected set; }
        
        public float ShootSpeed { get; protected set; }
        
        public float FireSpread { get; protected set; }
        
        public float SpreadFactor { get; protected set; }

        public LayerMask LayerMask { get; protected set; }

        public ParticleSystem MuzzleEffect { get; protected set; }

        public ParticleSystem Effect { get; protected set; }

        public float EffectDestroyDelay { get; protected set; }


        private List<IDisposable> _disposables = new();

        public ReactiveProperty<bool> IsReloadProcessing { get; protected set; }

        public bool IsShootReady { get; protected set; }

        public Laser Laser { get; private set; }


        public Weapon(int weaponId, GameObject weaponObject, Sprite weaponIcon, Projectile projectileObject,
            WeaponType weaponType, float damage, int clipSize, ReactiveProperty<int> leftPatronsCount,
            float reloadTime, float shootDistance, float shootSpeed, float fireSpread,
            LayerMask layerMask, ParticleSystem muzzleEffect, ParticleSystem effect, float effectDestroyDelay)
        {
            WeaponId = weaponId;
            WeaponObject = weaponObject;
            WeaponIcon = weaponIcon;
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
            MuzzleEffect = muzzleEffect;
            Effect = effect;
            EffectDestroyDelay = effectDestroyDelay;
            IsShootReady = true;
            IsReloadProcessing = new ReactiveProperty<bool>(false);

            Laser = new Laser(WeaponObject);
        }


        public virtual void Shoot(Transform playerTransform, Camera camera, Vector3 mousePosition)
        {
            RaycastAttack simpleAttack = new RaycastAttack(this, playerTransform, camera, mousePosition);
            simpleAttack.Attack();

            LeftPatronsCount.Value--;

            if (LeftPatronsCount.Value == 0)
                ProcessReload();

            IsShootReady = false;
            ProcessShootTimeout();

            Debug.Log("SHOOT MTFCKR");
            Laser.Blink(ShootSpeed);
        }


        public void Reload()
        {
            LeftPatronsCount.Value = ClipSize;
            Debug.Log("RELOAD MTFCKR");
            IsReloadProcessing.Value = false;
        }


        public void ProcessReload()
        {
            if (!IsReloadProcessing.Value)
            {
                IsReloadProcessing.Value = true;

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
