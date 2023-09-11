﻿using Abstracts;
using UnityEngine;


namespace User
{

    public class Weapon : IWeapon
    {
        public GameObject WeaponObject { get; }
        
        public WeaponType WeaponType { get; }
        
        public LayerMask LayerMask { get; }
        
        public ParticleSystem Effect { get; }

        public int WeaponId { get; }

        public  float Damage { get; }
        
        public float EffectDestroyDelay { get; }
        
        public int ClipSize { get; }
        
        public int LeftPatronsCount { get; }
        
        public float ReloadTime { get; }
        
        public float ShootDistance { get; }
        
        public float ShootSpeed { get; }
        
        public float FireSpread { get; }

        public virtual void Shoot()
        {
            
        }

        public Weapon(GameObject weaponObject, LayerMask layerMask, ParticleSystem effect, float damage, float effectDestroyDelay)
        {
            WeaponObject = weaponObject;
            LayerMask = layerMask;
            Effect = effect;
            Damage = damage;
            EffectDestroyDelay = effectDestroyDelay;
        }
        

    }
}
