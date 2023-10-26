using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using User;

public class PlayerAnimatorIK : MonoBehaviour
{

    [field:SerializeField] public Animator RootAnimator { get; set; }
    [field:SerializeField] public Animator _rigAnimator { get; set; }

    [field:SerializeField] public RigBuilder RigBuilder { get; set; }


    [field:SerializeField] public List<MultiAimConstraint> AimConstraints { get; set; }
     
    [field: SerializeField] public List<PlayerWeaponData> WeaponData { get; set; }
}

[Serializable]
public class PlayerWeaponData
{
    public WeaponType Type;
    public Transform WeaponHolder;
}
