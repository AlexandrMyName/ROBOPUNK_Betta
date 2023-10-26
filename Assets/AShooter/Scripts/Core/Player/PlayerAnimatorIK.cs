using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimatorIK : MonoBehaviour
{

    [field:SerializeField] public Animator RootAnimator { get; set; }
    [field:SerializeField] public Animator _rigAnimator { get; set; }

    [field:SerializeField] public RigBuilder RigBuilder { get; set; }

    
}
