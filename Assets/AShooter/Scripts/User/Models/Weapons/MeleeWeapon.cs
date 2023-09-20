﻿using System;
using Abstracts;
using Core;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;


namespace User
{

    public abstract class MeleeWeapon : IMeleeWeapon, IDisposable
    {
        
        public int WeaponId { get; }
        
        public GameObject WeaponObject { get; }
        
        public WeaponType WeaponType { get; }
        
        public float Damage { get; }
        
        public LayerMask LayerMask { get; }
        
        public ParticleSystem Effect { get; }
        
        public float EffectDestroyDelay { get; }
        
        public float AttackTimeout { get; }
        
        public bool IsAttackReady { get; private set; }
        

        private List<IDisposable> _disposables = new();

        private Collider[] _hitColliders;



        public MeleeWeapon(int weaponId, GameObject weaponObject, WeaponType weaponType, float damage, 
            LayerMask layerMask, ParticleSystem effect, float effectDestroyDelay, float attackTimeout)
        {
            WeaponId = weaponId;
            WeaponObject = weaponObject;
            WeaponType = weaponType;
            Damage = damage;
            LayerMask = layerMask;
            Effect = effect;
            EffectDestroyDelay = effectDestroyDelay;
            AttackTimeout = attackTimeout;
            IsAttackReady = true;
        }
        
        
        public void Attack()
        {
            IsAttackReady = false;
            
            ProcessAttackTimeout();
            PerformRayAttack();
        }
        
        
        public void ProcessAttackTimeout()
        {
            if (!IsAttackReady)
            {
                _disposables.Add(
                    Observable
                        .Timer(TimeSpan.FromSeconds(AttackTimeout))
                        .Subscribe(_ => IsAttackReady = true)
                );
            }
        }
        
        
        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
        }


        private void PerformRayAttack()
        {
            var ray = new Ray(WeaponObject.transform.position, WeaponObject.transform.forward);
            var distance = 0.5f;
            var boxHalfExtents = Vector3.one;

            _hitColliders = Physics.OverlapBox(
                WeaponObject.transform.position, 
                boxHalfExtents, 
                Quaternion.identity,
                LayerMask);

            if (_hitColliders.Length > 0)
            {
                var instances = _hitColliders.Select(c => c.gameObject.GetInstanceID()).ToList();
            }


            for (int i = 0; i < _hitColliders.Length; i++)
            {
                if (_hitColliders[i].TryGetComponent(out IEnemy unit))
                {
                    Debug.Log($"Found target [{unit}] health {unit.ComponentsStore.Attackable.Health}");
                    unit.ComponentsStore.Attackable.TakeDamage(Damage);
                }
            }
            
        }


        public void DrawBoxCast()
        {
            var ray = new Ray(WeaponObject.transform.position, WeaponObject.transform.forward);
            var distance = 0.5f;
            
            var boxHalfExtents = Vector3.one;
            
            if (Physics.BoxCast(ray.origin, boxHalfExtents, ray.direction, out var hitInfo, Quaternion.identity, distance, LayerMask))
            {
                DrawRay(ray, ray.origin + ray.direction * hitInfo.distance, hitInfo.distance, Color.red);
            }
            else
            {
                DrawRay(ray, ray.origin + ray.direction * distance, distance, Color.green);
            }
        }
        
        
        private void DrawRay(Ray ray, Vector3 hitPosition, float distance, Color color)
        {
            
            Debug.DrawRay(ray.origin, ray.direction * distance, color);

            Gizmos.color = color;

            Gizmos.DrawWireCube(hitPosition, Vector3.one * 2.0f);
        }
        
        
    }
}
