using System;
using System.Collections.Generic;
using Abstracts;
using UniRx;
using UnityEngine;
using Zenject;


namespace Core
{
    
    public sealed class PlayerRotationSystem : BaseSystem , IDisposable
    {

        [Inject] private IInput _input;
        private Player _player;
        private Rigidbody _rigidbody;
        private Camera _camera;
        private Vector3 _closestHitPoint;

        private List<IDisposable> _disposables = new();

        private Quaternion _rotation = Quaternion.identity;
        private Vector3 _direction;
        

        protected override void Awake(IGameComponents components)
        {
            _player = components.BaseObject.GetComponent<Player>();
            _rigidbody = _player.GetComponent<Rigidbody>();
            _camera = Camera.main;
        }
        

        protected override void Start()
        {
            _disposables.AddRange(new List<IDisposable>
                {
                    _input.MousePosition.AxisOnChange.Subscribe(OnMousePositionChanged)
                }
            );
        }
        

        private void OnMousePositionChanged(Vector3 position)
        {
            var ray = _camera.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject != _player.gameObject)
            {
                _closestHitPoint = hit.point;
                SetDirection();
                SetRotation();
            }
        }

        protected override void FixedUpdate()
        {

            _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, Quaternion.Euler(0, _rotation.eulerAngles.y, 0),10 * Time.fixedDeltaTime);
        }


        private void SetDirection()
        {
            _direction = _closestHitPoint - _player.transform.position;
        }


        private void SetRotation()
        {
            _rotation = Quaternion.LookRotation(_direction, Vector3.up);
        }

         
        public void Dispose() => _disposables.ForEach(d => d.Dispose());

    }

}