﻿using System;
using Abstracts;
using Core;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


namespace User
{

    public class Weapon : IRangeWeapon
    {
        
        public int WeaponId { get; protected set; }

        public GameObject WeaponObject { get; protected set; }

        public Sprite WeaponIcon { get; protected set; }

        public Projectile ProjectileObject { get; protected set; }

        public float ProjectileForce { get; protected set; }
        
        public WeaponType WeaponType { get; protected set; }
        
        public float Damage { get; set; }

        public int ClipSize { get; protected set; }

        public ReactiveProperty<int> LeftPatronsCount { get; protected set; }

        public ReactiveProperty<int> TotalPatrons { get; set; }

        public int TotalPatronsMaxCount { get; protected set; }

        public float ReloadTime { get; protected set; }
        
        public float ShootDistance { get; protected set; }
        
        public float ShootSpeed { get; set; }
        
        public float FireSpread { get; protected set; }
        
        public float SpreadFactor { get; protected set; }

        public LayerMask LayerMask { get; protected set; }

        public ParticleSystem MuzzleEffect { get; protected set; }

        public float MuzzleEffectDestroyDelay { get; protected set; }

        public ParticleSystem Effect { get; protected set; }

        public float EffectDestroyDelay { get; protected set; }

        public GameObject FakeWeaponObject { get; protected set; }

        public GameObject FakeBulletsObject { get; protected set; }

        public ReactiveProperty<bool> IsReloadProcessing { get; protected set; }

        public bool IsShootReady { get; protected set; }

        public Laser Laser { get; private set; }


        private List<IDisposable> _disposables = new();

        private RaycastAttack _attackTypeProcess;


        public Weapon(int weaponId, GameObject weaponObject, Sprite weaponIcon, Projectile projectileObject, float projectileForce,
            WeaponType weaponType, float damage, int clipSize, ReactiveProperty<int> leftPatronsCount, int totalPatronsMaxCount,
            float reloadTime, float shootDistance, float shootSpeed, float fireSpread, float spreadFactor, LayerMask layerMask,
            ParticleSystem muzzleEffect, float muzzleEffectDestroyDelay, ParticleSystem effect, float effectDestroyDelay,
            GameObject fakeWeaponObject, GameObject fakeBulletsObject, Camera camera)
        {
            WeaponId = weaponId;
            WeaponObject = weaponObject;
            WeaponIcon = weaponIcon;
            ProjectileObject = projectileObject;
            ProjectileForce = projectileForce;
            WeaponType = weaponType;
            Damage = damage;
            ClipSize = clipSize;
            LeftPatronsCount = leftPatronsCount;
            TotalPatronsMaxCount = totalPatronsMaxCount;
            ReloadTime = reloadTime;
            ShootDistance = shootDistance;
            ShootSpeed = shootSpeed;
            FireSpread = fireSpread;
            SpreadFactor = spreadFactor;
            LayerMask = layerMask;
            MuzzleEffect = muzzleEffect;
            MuzzleEffectDestroyDelay = muzzleEffectDestroyDelay;
            Effect = effect;
            EffectDestroyDelay = effectDestroyDelay;
            FakeWeaponObject = fakeWeaponObject;
            FakeBulletsObject = fakeBulletsObject;
            IsShootReady = true;
            IsReloadProcessing = new ReactiveProperty<bool>(false);

            Laser = new Laser(WeaponObject);
            _attackTypeProcess = new RaycastAttack(this, camera);
            TotalPatrons = new ReactiveProperty<int>(leftPatronsCount.Value);
        }


        public virtual void Shoot(Vector3 mousePosition)
        {
            _attackTypeProcess.Attack(mousePosition);

            TotalPatrons.Value--;
            LeftPatronsCount.Value--;

            if ((LeftPatronsCount.Value == 0) && ((WeaponType == WeaponType.Pistol) || (TotalPatrons.Value != 0)))
                ProcessReload();

            IsShootReady = false;
            ProcessShootTimeout();

            //Laser.Blink(ShootSpeed);
        }


        public void Reload()
        {
            if (WeaponType == WeaponType.Pistol)
                TotalPatrons.Value = TotalPatrons.Value > ClipSize ? TotalPatrons.Value : ClipSize;

            LeftPatronsCount.Value = Math.Clamp(ClipSize, 0, TotalPatrons.Value);
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
                            .AddTo(_disposables)
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
                        .AddTo(_disposables)
                );
            }
        }


        public void Dispose()
        {
            Laser.Dispose();
            _disposables.ForEach(d => d.Dispose());
        }
        
        
    }
}
