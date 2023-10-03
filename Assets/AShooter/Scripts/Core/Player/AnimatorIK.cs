using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using System;
using UniRx;

namespace Core
{

    public class AnimatorIK : MonoBehaviour
    {

        [Header("This contains data of RB && animations with IK")]
        [Space]
        [SerializeField, Range(0, 1f)] float _weight, _body, _head, _eyes, _clamp;

        [SerializeField] private LayerMask _rbLayer;
        [SerializeField] private TypeOfAnimation _animType;

        private Animator _animator;
        private Vector3 _lookAtIKpos;

        private List<Rigidbody> _ragdoll = new();


        /// <summary>
        /// This method will determine which animation to use,
        /// please check the animation type on the prefab
        /// </summary>
        /// <param name="hitPoint">raycast data from attack</param>
        /// <param name="attackDirection">direction without inverse</param>
        public void ActivateDeathAnimation(RaycastHit hitPoint, Vector3 attackDirection)
        {

            if (hitPoint.collider.GetComponent<Rigidbody>() == null)
                throw new NullReferenceException("Rigidbody is null (AnimatorIK on prefab") ;

            if (_animType != TypeOfAnimation.None)
            {
                GetComponent<Collider>().isTrigger = true;
                GetComponent<Rigidbody>().isKinematic = true;

            }

            switch (_animType)
            {
                case TypeOfAnimation.Humanoid:
                    ActivateHumanoidDeath(hitPoint, attackDirection);
                    break;

                case TypeOfAnimation.NonHumanoid:

                    break;

                default:
                    gameObject.SetActive(false);
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
        public void SetBool(string keyID, bool value) => _animator.SetBool(keyID, value);


        private void OnAnimatorIK(int layerIndex)
        {
            _animator.SetLookAtWeight(_weight, _body, _head, _eyes, _clamp);
            if (_lookAtIKpos != null)
                _animator.SetLookAtPosition(_lookAtIKpos);
        }


        private void ActivateHumanoidDeath(RaycastHit hitPoint, Vector3 attackDirection)
        {

            _ragdoll.ForEach(rb =>
            {

                var collider = rb.gameObject.GetComponent<Collider>();
                collider.isTrigger = false;
                rb.isKinematic = false;

            });

            hitPoint.collider
                .GetComponent<Rigidbody>()
                .AddForce(
                    attackDirection,
                    ForceMode.Impulse
            );
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
                if (rb.gameObject.layer == _rbLayer)
                    _ragdoll.Add(rb);
            }
            return _ragdoll.Count;
        }


        private void SetDeactivateTimer(float seconds = default)
        {

            if(seconds != default)
            {
                Observable.Timer(TimeSpan.FromSeconds(seconds)).Subscribe(
                    
                    val=>
                    {
                        gameObject.SetActive(false);
                    });
            }
            else gameObject.SetActive(false);
        }
    }
}