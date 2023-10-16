using Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using User;


[Serializable]
public class WeaponRaycast
{

    class Bullet
    {

        public Vector3 initPosition;
        public Vector3 initVelocity;
        public float time = 0.0f;
        public BulletConfig config;
        public TrailRenderer tracer;
    }
    private List<Bullet> _bullets = new();


    [SerializeField] private ParticleSystem[] _muzzleEffects;
    [SerializeField] private Transform _muzzleTransform;

    private ParticleSystem _hitEffect;
    private TrailRenderer _trailRenderer;

    private Ray _ray;
    RaycastHit hit;
    private float _weaponDamage;


    public void InitEffects(
        TrailRenderer trailRenderer,
        ParticleSystem hitEffect,
        float weaponDamage){

        _weaponDamage = weaponDamage;
        _hitEffect = GameObject.Instantiate(hitEffect,null,true);
        _trailRenderer = trailRenderer;
        
    }


    public void UpdateBullets(float deltaTime)
    {

        SimulationBullets(deltaTime);
        DestroyBullets(deltaTime);
    }


    public void Shoot(BulletConfig config)
    {

        foreach(var effect in _muzzleEffects) { effect.Emit(1); }

        FireBullet(_hitEffect, config);

    }

    private Vector3 GetPosition(Bullet bullet)
    {
        // pos + (velocity * time) + .5f * gravity * time * time

        Vector3 gravity = Vector3.down * bullet.config.Drop;
        return (bullet.initPosition) + (bullet.initVelocity * bullet.time) + (.5f * gravity * bullet.time * bullet.time);
    }


    private Bullet CreateBullet(
           Vector3 initPosition,
           Vector3 initVelocity,
           BulletConfig config){

            ///[Method]\\\

        var bullet = new Bullet();
        bullet.initPosition = initPosition;
        bullet.initVelocity = initVelocity;
        bullet.time = 0.0f;
        bullet.config = config;

        if (bullet.config.Traicer != null)
        {

            bullet.tracer = GameObject.Instantiate(config.Traicer, initPosition, Quaternion.identity);
            bullet.tracer.AddPosition(initPosition);
        }
        return bullet;
    }


    private void SimulationBullets(float deltaTime)
    {

        _bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;

            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }


    private void DestroyBullets(float deltaTime)
      => _bullets.RemoveAll(bullet => bullet.time >= bullet.config.MaxTime);


    private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {

        Vector3 direction = end - start;
        float distance = direction.magnitude;

        _ray.origin = start;
        _ray.direction = direction;

        if (Physics.Raycast(_ray, out hit, distance))
        {
            //we can check material collider and sets hit effect from config (need create)
            TryInstantiateHitEffect();

            TryInstantiateTracer(bullet);

            if (hit.collider.TryGetComponent<IPlayer>(out var player))
            {
                player.ComponentsStore.Attackable.TakeDamage(_weaponDamage);
            }

            bullet.time = bullet.config.MaxTime;
        }
        else if (bullet.tracer != null) bullet.tracer.transform.position = end;
    }


    private void TryInstantiateHitEffect()
    {

        if (_hitEffect != null)
        {
            CheckHitLayers(out var canActiveHit);

            if (canActiveHit)
            {
                var effect = GameObject.Instantiate(_hitEffect);
                effect.transform.position = hit.point;
                effect.transform.forward = hit.normal;
                effect.Emit(1);
            }
        }
    }


    private void TryInstantiateTracer(Bullet bullet)
    {

        if (bullet.tracer != null)
            bullet.tracer.transform.position = hit.point;
    }


    private void CheckHitLayers(out bool canActiveHit) // Need add method of finder MaskToNumbers
    {

         canActiveHit = true;
        //int[] ignoreHitEffectsLayers = _ignoreRaycastLayerMask.MaskToNumbers();

        //foreach (int layerIndex in ignoreHitEffectsLayers)
        //{
        //    if (hit.collider.gameObject.layer == layerIndex)
        //    {
        //        canActiveHit = false;
        //    }
        //}
    }


    private void FireBullet(ParticleSystem hitEffect, BulletConfig config)
    {
  
        Vector3 velocity = (_muzzleTransform.position  -  _muzzleTransform.position + _muzzleTransform.forward * 40).normalized  * config.Speed;
        
        var bullet = CreateBullet(_muzzleTransform.position, velocity, config);

        _bullets.Add(bullet);
    }
}
