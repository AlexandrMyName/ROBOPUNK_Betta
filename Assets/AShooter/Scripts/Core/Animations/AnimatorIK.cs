using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using System;
using UniRx;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using Zenject;
using User;

namespace Core
{

    public class AnimatorIK : MonoBehaviour, IAnimatorIK
    {

        [Header("This contains data of RB && animations with IK")]
        [Space]
       
        [SerializeField] private int _indexLayerOfRagdoll = 10;
        [SerializeField] private TypeOfAnimation _animType;
        [SerializeField] private Collider _baseCollider;
        [SerializeField] private Rigidbody _baseRigidbody;

        [Header("IK can be null, data of Aiming"),Space(5)]
        [SerializeField] private MultiAimConstraint _aimConstraint;
        [SerializeField] private RigBuilder _rigBuilder;
        [SerializeField] private List<WeaponIK> _weaponIK;
        [SerializeField] private bool _isEnemy;

        private IEnemy _enemy;
        private Animator _animator;
        private Vector3 _lookAtIKpos;
        private WeaponIK _currentWeapon;
        private bool _isAimingAnimation;
        private List<RagdollData> _ragdoll = new();


        /// <summary>
        /// This method will determine which animation to use,
        /// please check the animation type on the prefab
        /// </summary>
        /// <param name="hitPoint">raycast data from attack</param>
        /// <param name="attackDirection">direction without inverse</param>
        public void ActivateDeathAnimation(RaycastHit hitPoint, Vector3 attackDirection)
        {

            switch (_animType)
            {
                case TypeOfAnimation.Humanoid:
                    ActivateHumanoidDeath(hitPoint, attackDirection);
                    SetDeactivateTimer(5);
                    break;

                case TypeOfAnimation.NonHumanoid:
                    ActivateNoneHumanoidDeath(attackDirection);
                    SetDeactivateTimer(10);
                    break;

                default:
                    SetDeactivateTimer(default);
                    break;
            }

        }


        public void SetLayerWeight(int indexLayer, float weight)
        => _animator.SetLayerWeight(indexLayer, weight);
        
         
        public void SetLookAtPosition(Vector3 lookAt) => _lookAtIKpos = lookAt;

        public void SetTrigger(string keyID) => _animator.SetTrigger(keyID);

        public void SetFloat(string keyID, float value) => _animator.SetFloat(keyID, value);

        public void SetFloat(string keyID, float value,float delta) 
        => _animator.SetFloat(keyID, value,.0f, delta);

        public void SetBool(string keyID, bool value) => _animator.SetBool(keyID, value);

  
        private void ActivateHumanoidDeath(RaycastHit hitPoint, Vector3 attackDirection)
        {

            GetComponent<NavMeshAgent>().enabled = false;

            SetActiveAnimator(false);
            SetRigsWeight(0,0,0);
            SetActiveRagdoll(true);

            //if (hitPoint.rigidbody != null)
            //{

            //    hitPoint.collider
            //        .GetComponent<Rigidbody>()
            //        .AddForce(
            //            attackDirection,
            //            ForceMode.Impulse
            //    );

            //}
        }


        private void SetRigsWeight(float noneAiming = 0, float aimingWeight = 0, float handsWeight = 0)
        {

            bool isDisable = noneAiming == 0 && aimingWeight == 0 && handsWeight == 0;

            foreach (var weapon in _weaponIK)
            {

                weapon.DefaultRig.weight = noneAiming;
                weapon.AimingRig.weight = aimingWeight;
                weapon.HandsRig.weight = handsWeight;

                 weapon.InternalObjectWithCollider.SetActive(!isDisable);
            }
        }


        public void SetAimingAnimation(bool isActive, WeaponType weaponType)
        {

            if (_weaponIK == null || _weaponIK.Count == 0) return;

            if (weaponType == default)
                 _currentWeapon = _weaponIK[0];
            else
            {
                _currentWeapon = _weaponIK.Find(weapon => weapon.Type == weaponType);
            } 
             

            _isAimingAnimation = isActive;
        }


    


        private void ActivateNoneHumanoidDeath(Vector3 attackDirection)
        {
            
            TrySetActiveSpiderIK(false);
            SetActiveAnimator(false);
            SetActiveRagdoll(true);
 
        }


        private void OnValidate()
        {
            _baseCollider ??= GetComponent<Collider>();
            _baseRigidbody ??= GetComponent<Rigidbody>();
        }


        private void Awake()
        {

            if (_isEnemy)
            {
                _enemy = GetComponent<IEnemy>();
            }

            _animator = GetComponent<Animator>();
             
            if(_aimConstraint != null)
            {
                
                WeightedTransformArray ikArray = new WeightedTransformArray(0);

                Transform playerTarget = GameObject.Find("Player").GetComponent<Transform>();// Zenject  ++
                 
                ikArray.Add(new WeightedTransform( playerTarget , 1f));

                _aimConstraint
                   .data
                    .sourceObjects = ikArray;
                
                _rigBuilder.Build();
            }
        }


        private void Start()
        {

            if (TryGetRagDoll(20) == null)
                Debug.LogWarning("Ragdoll is empty , please look for layer in plrefab");

            if(_animType == TypeOfAnimation.Humanoid)
            {
                foreach(var ragdollData in _ragdoll)
                {
                    ragdollData.Rb.isKinematic = true;
                    ragdollData.Rb.GetComponent<Collider>().isTrigger = true;
                }
            }
        }


        private void Update()
        {

            if (_isEnemy == true && _animType == TypeOfAnimation.Humanoid)
            {
                _animator.SetBool("Move", _enemy.EnemyState == DTO.EnemyState.Walk ? true : false);
            }


            if (_currentWeapon == null) return;
                UpdateAimingState();
        }


        private void UpdateAimingState()
        {
            if (_isAimingAnimation)
            {

                _currentWeapon.AimingRig.weight += Time.deltaTime / _currentWeapon.AimingDuration;
                _currentWeapon.DefaultRig.weight -= Time.deltaTime / _currentWeapon.AimingDuration;
            }
            else
            {
                _currentWeapon.AimingRig.weight -= Time.deltaTime / _currentWeapon.AimingDuration;
                _currentWeapon.DefaultRig.weight += Time.deltaTime / _currentWeapon.AimingDuration;
            }
        }


        private int? TryGetRagDoll(float weight = 1)
        {

            var rbs = GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < rbs.Length; i++)
            {
                var rb = rbs[i];
                if ( _indexLayerOfRagdoll == rb.gameObject.layer )
                    _ragdoll.Add(
                        new RagdollData(
                            rb.gameObject.transform.localPosition,
                            rb.gameObject.transform.localRotation,
                            rb)
                        );
               // rb.mass = weight;
            }
            Debug.LogWarning(rbs.Length);
            return _ragdoll.Count;
        }


        private void SetDeactivateTimer(float seconds = default)
        {

            if (seconds != default)
            {
                Observable.Timer(TimeSpan.FromSeconds(seconds)).Subscribe(

                    val =>
                    {
                        SetActiveRagdoll(false);

                        SetActiveAnimator(true);

                        TrySetActiveSpiderIK(true);

                        TryCompletelyDeath();

                    });
            }
            else TryCompletelyDeath();
        }


        private void TryCompletelyDeath() 
        {

            IEnemy enemy = GetComponent<IEnemy>();
             
            GetComponent<NavMeshAgent>().enabled = true;

            SetRigsWeight(0,1,1);
            if (enemy != null)
                enemy
                    .ComponentsStore
                    .Attackable
                    .IsDeadFlag.Value = true;
            else
            {
                gameObject.SetActive(false);
            }
        }


        private void SetActiveRagdoll(bool isActive)
        {

            _ragdoll.ForEach(data =>
            {

                var collider = data.Rb.gameObject.GetComponent<Collider>();
                 
                collider.isTrigger = !isActive;
                data.Rb.isKinematic = !isActive;
                data.Rb.gameObject.transform.localPosition =  data.CachedPosition;
                data.Rb.gameObject.transform.localRotation =  data.CachedRotation;
              
            });
        }


        private void SetActiveAnimator(bool isActive)
        {

            if (_animator == null)
                Debug.LogWarning("You try use animator in AnimatorIK , check this script ");
            else _animator.enabled = isActive;

        }


        private void TrySetActiveSpiderIK(bool isActive)
        {
            
            var spiderIK = GetComponentInChildren<SpiderMec>();
            if(spiderIK == null)
                spiderIK =  GetComponent<SpiderMec>();


            if(spiderIK != null)
            {
                spiderIK.enabled = isActive;
                var agent = GetComponent<NavMeshAgent>().enabled = isActive;
                _baseCollider.isTrigger = !isActive;
            }
            else
            {
                //Not spider
            }
        }
    }
}