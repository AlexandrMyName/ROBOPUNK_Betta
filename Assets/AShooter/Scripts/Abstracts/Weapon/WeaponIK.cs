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

        [Range(.1f,1f)] public float AimingDuration;
    }
}