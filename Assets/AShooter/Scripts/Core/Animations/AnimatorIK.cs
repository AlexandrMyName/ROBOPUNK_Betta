using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using System;
using UniRx;
using UnityEngine.AI;


namespace Core
{

    public class AnimatorIK : MonoBehaviour, IAnimatorIK
    {

        [Header("This contains data of RB && animations with IK")]
        [Space]
        [SerializeField, Range(0, 1f)] float _weight, _body, _head, _eyes, _clamp;

        [SerializeField] private int _indexLayerOfRagdoll = 10;
        [SerializeField] private TypeOfAnimation _animType;
        [SerializeField] private Collider _baseCollider;
        [SerializeField] private Rigidbody _baseRigidbody;

        private Animator _animator;
        private Vector3 _lookAtIKpos;

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
                    SetDeactivateTimer(10);
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
        

        public void SetLookAtWeight(float weight, float body, float head, float eyes, float clamp)
        {

            _weight = weight;
            _body = body;
            _head = head;
            _eyes = eyes;
            _clamp = clamp;
        }


        public void SetLookAtPosition(Vector3 lookAt) => _lookAtIKpos = lookAt;

        public void SetTrigger(string keyID) => _animator.SetTrigger(keyID);

        public void SetFloat(string keyID, float value) => _animator.SetFloat(keyID, value);

        public void SetFloat(string keyID, float value,float delta) 
        => _animator.SetFloat(keyID, value,.0f, delta);

        public void SetBool(string keyID, bool value) => _animator.SetBool(keyID, value);


        private void OnAnimatorIK(int layerIndex)
        {

            _animator.SetLookAtWeight(_weight, _body, _head, _eyes, _clamp);
            if (_lookAtIKpos != null)
                _animator.SetLookAtPosition(_lookAtIKpos);
        }


        private void ActivateHumanoidDeath(RaycastHit hitPoint, Vector3 attackDirection)
        {

            SetActiveAnimator(false);
            SetActiveRagdoll(true);

            if (hitPoint.rigidbody != null)
            {

                hitPoint.collider
                    .GetComponent<Rigidbody>()
                    .AddForce(
                        attackDirection,
                        ForceMode.Impulse
                );

            }
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


        private void Awake() => _animator = GetComponent<Animator>();


        private void Start()
        {

            if (TryGetRagDoll() == null)
                Debug.LogWarning("Ragdoll is empty , please look for layer in plrefab");
        }


        private int? TryGetRagDoll()
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

                    });
            }
            else TryCompletelyDeath();
        }


        private void TryCompletelyDeath()// agent should be here
        {

            IEnemy enemy = GetComponent<IEnemy>();

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