﻿using System;
using Abstracts;
using Core;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Tool;
using UniRx;
using UnityEngine;


namespace User
{

    public abstract class Weapon : IWeapon, IDisposable
    {

        // private Coroutine _reloadCoroutine;


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


        private List<IDisposable> _disposables = new();


        public virtual void Shoot(Transform playerTransform, Camera camera, Vector3 mousePosition)
        {
            PlayerSimpleAttack simpleAttack = new PlayerSimpleAttack(this, playerTransform, camera, mousePosition);
            simpleAttack.Attack();

            LeftPatronsCount--;
            Debug.Log("SHOOT MTFCKR");
        }


        public void Reload()
        {
            LeftPatronsCount = ClipSize;
            Debug.Log("RELOAD MTFCKR");
        }


        public void ProcessReload()
        {
            _disposables.Add(
                Observable
                    .Timer(TimeSpan.FromSeconds(ReloadTime))
                    .Subscribe(_ => Reload())
                );
        }
        
        
        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
        }

        
        // private IEnumerator ReloadCoroutine()
        // {
        //     yield return new WaitForSeconds(ReloadTime);
        //     
        // }
        
    }
}
