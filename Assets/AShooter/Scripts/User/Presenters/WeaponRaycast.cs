using Abstracts;
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WeaponRaycast
{

    [SerializeField] private ParticleSystem[] _muzzleEffects;
    [SerializeField] private Transform _muzzleTransform;

    private ParticleSystem _hitEffect;
    private TrailRenderer _trailRenderer;

    private Ray _ray;
    private float _weaponDamage;


    public void InitEffects(TrailRenderer trailRenderer, ParticleSystem hitEffect, float weaponDamage)
    {

        _weaponDamage = weaponDamage;
        _hitEffect = GameObject.Instantiate(hitEffect,null,true);
        _trailRenderer = trailRenderer;
        
    }


    public void Shoot()
    {

        foreach(var effect in _muzzleEffects)
        {
            effect.Emit(1);
        }
         
        _ray.origin = _muzzleTransform.position;
        _ray.direction = _muzzleTransform.forward;

        float xRandom = Random.Range(-.3f, .3f);
        float yRandom = Random.Range(-.3f, .3f);
        float zRandom = Random.Range(-.3f, .3f);
        _ray.direction += new Vector3(zRandom, xRandom, yRandom);

        var trail = GameObject.Instantiate(_trailRenderer,_ray.origin,Quaternion.identity);
        trail.transform.position = _muzzleTransform.position;
        trail.AddPosition(_ray.origin);


        var hits = Physics.RaycastAll(_ray);

        if (hits.Length > 0)
        {
            var hitOrdered = hits.Where(hit => hit.collider.isTrigger == false)
                .OrderBy(h=> Vector3.Distance(h.point, _muzzleTransform.position));

            var hit = hitOrdered.FirstOrDefault();

                _hitEffect.transform.position = hit.point;
                _hitEffect.transform.forward = hit.normal;
                _hitEffect.Emit(1);
                trail.transform.position = hit.point;

            if (hit.collider == null) return;

              hit.collider.TryGetComponent<IPlayer>( out var player);

            if(player != null)
            {
                
                player.ComponentsStore.Attackable.TakeDamage(_weaponDamage);
            }
        }
        
     
    }

}
