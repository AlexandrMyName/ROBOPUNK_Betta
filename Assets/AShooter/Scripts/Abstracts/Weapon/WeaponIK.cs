using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using User;


namespace Abstracts
{

    [Serializable]
    public class WeaponIK
    {

        public GameObject InternalObjectWithCollider;
        public WeaponConfig Config;
        public WeaponType Type;
        public Rig HandsRig;
        public Rig AimingRig;
        public Rig DefaultRig;
        public WeaponRaycast Muzzle;
        public BulletConfig BulletsConfig;
        public TrailRenderer TrailRendererPrefab;
        [Range(.1f,1f)] public float AimingDuration;


        public void InitWeapon()
        {
            if(Muzzle != null && TrailRendererPrefab && Config)
                Muzzle.InitEffects(TrailRendererPrefab, Config.Effect, Config.Damage);
        }
    }
}