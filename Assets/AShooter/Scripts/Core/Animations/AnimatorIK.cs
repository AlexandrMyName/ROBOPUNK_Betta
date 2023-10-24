using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using System;
using UniRx;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using User;
using Core.DTO;

namespace Core
{

    public class AnimatorIK : MonoBehaviour, IAnimatorIK, IDisposable
    {

        [Header("This contains data of RB && animations with IK")]
        [Space]
       
        [SerializeField] private int _indexLayerOfRagdoll = 10;
        [SerializeField] private TypeOfAnimation _animType;
        [SerializeField] private Collider _baseCollider;
        [SerializeField] private Rigidbody _baseRigidbody;

        [Header("IK can be null, data of Aiming"),Space(5)]
        [SerializeField] private List<MultiAimConstraint> _aimConstraints;
        [SerializeField] private RigBuilder _rigBuilder;
        [SerializeField] private List<WeaponIK> _weaponIK;
        [SerializeField] private GameObject _inernalShield;
        [SerializeField] private bool _isEnemy;


        private IEnemy _enemy;
        private Animator _animator;
        private Vector3 _lookAtIKpos;
        private WeaponIK _currentWeapon;
        private bool _isAimingAnimation;
        private List<RagdollData> _ragdoll = new();

        private AudioSource _audioSource;
        private List<IDisposable> _disposables = new();

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
                    SetDeactivateTimer(14);
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


        public void UpdateShieldObject(bool isActive)
        {

            if (_inernalShield != null)
            {

                // Can be animation || effects


                var hitEffets = gameObject.GetComponentsInChildren<ParticleSystem>();

                foreach(var hitEffet in hitEffets)
                {
                    Destroy(hitEffet.gameObject);
                }

                _inernalShield.SetActive(isActive);
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

  
        public void ShootIK() {

            if (_currentWeapon != null)
            {
                _currentWeapon.Muzzle.Shoot(_currentWeapon.BulletsConfig);

                PlaySound(_audioSource, _currentWeapon);
            }
            if (_currentWeapon != null && _currentWeapon.AimingRig.weight > .7f)
            {
                _currentWeapon.Muzzle.Shoot(_currentWeapon.BulletsConfig);
            }
        }


        private void ActivateHumanoidDeath(RaycastHit hitPoint, Vector3 attackDirection)
        {

            GetComponent<NavMeshAgent>().enabled = false;

            SetActiveAnimator(false);
            SetRigsWeight(0,0,0);
            SetActiveRagdoll(true);
 
        }


        private void SetRigsWeight(float noneAiming = 0, float aimingWeight = 0, float handsWeight = 0)
        {

            bool isDisable = noneAiming == 0 && aimingWeight == 0 && handsWeight == 0;

            foreach (var weapon in _weaponIK)
            {
                if(weapon.DefaultRig)
                    weapon.DefaultRig.weight = noneAiming;
                if (weapon.AimingRig)
                    weapon.AimingRig.weight = aimingWeight;
                if (weapon.HandsRig)
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

            SetRigsWeight(0, 0, 0);
            TrySetActiveSpiderIK(false);
            SetActiveAnimator(false);
            SetActiveRagdoll(true);
 
        }

        #region Unity_Methods

        private void OnValidate()
        {

            _baseCollider ??= GetComponent<Collider>();
            _baseRigidbody ??= GetComponent<Rigidbody>();
        }


        private void Awake()
        {

            _weaponIK.ForEach(weapon => weapon.InitWeapon());

            if (_isEnemy)
            {
                _enemy = GetComponent<IEnemy>();
            }

            _animator = GetComponent<Animator>();
             
            if(_aimConstraints != null)
            {
                foreach (var aim in _aimConstraints)
                {
                    WeightedTransformArray ikArray = new WeightedTransformArray(0);

                    Transform playerTarget = GameObject.Find("Player").GetComponent<Transform>();// Zenject  ++

                    ikArray.Add(new WeightedTransform(playerTarget, 1f));

                    aim
                       .data
                        .sourceObjects = ikArray;
                     
                }
                if(_rigBuilder != null)
                    _rigBuilder.Build();
            }

            _audioSource = gameObject.GetComponent<AudioSource>();
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

            if (_animator != null)
            {

                if (_currentWeapon == null) return;
                UpdateAimingState();
 
               _currentWeapon.Muzzle.UpdateBullets(Time.deltaTime);
                 
                if (_animator.runtimeAnimatorController == null) return;


                if (_isEnemy == true && _animType == TypeOfAnimation.Humanoid)
                {
                    _animator.SetBool("Move", _enemy.EnemyState == DTO.EnemyState.Walk ? true : false);
                }


                
            }
        }

        #endregion

        private void UpdateAimingState()
        {

            if (_currentWeapon.AimingRig == null) return;

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

                    }).AddTo(_disposables);
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
                if(_baseCollider != null)
                    _baseCollider.isTrigger = !isActive;
            }
            else
            {

                var agent = GetComponent<NavMeshAgent>().enabled = isActive;
                if (_baseCollider != null)
                    _baseCollider.isTrigger = !isActive;
            }
        }


        private void PlaySound(AudioSource audioSource, WeaponIK weapon)
        {
            if ((weapon != null) && (audioSource != null))
            {
                var soundModelType = weapon.Type switch
                {
                    WeaponType.Sword => SoundModelType.Weapon_Sword,
                    WeaponType.Rifle => SoundModelType.Weapon_Rifle,
                    _ => SoundModelType.None
                };

                if (soundModelType != SoundModelType.None)
                {
                    AudioClip audioClip = SoundManager.Config.GetSound(SoundType.Damage, soundModelType);

                    if (audioClip != null)
                        audioSource.PlayOneShot(audioClip);
                }
            }
        }

        public void Dispose()
        {
            _disposables.ForEach(disp=>disp.Dispose()); 
        }

        private void OnDestroy()
        {
            Dispose();
            _disposables.Clear();
        }
    }
}