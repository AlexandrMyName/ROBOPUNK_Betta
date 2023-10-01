using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;


namespace User
{
    
    public sealed class Laser : IDisposable
    {

        private GameObject _laserObject;
        private LineRenderer _lineRenderer;
        private Transform _laserTransform;
        
        private Vector3 _endLineVectorDefault = Vector3.zero;
        private Vector3 _endLineVector = Vector3.zero;
        private Vector3 _hitVector;

        private float _distance;

        private bool _isLaserExist;

        private List<IDisposable> _disposables;


        public Laser(GameObject weaponObject)
        {
            if (FindLaserIfExists(weaponObject))
                InitLaserObject();
        }


        public void Update()
        {
            if (_isLaserExist)
            {
                _hitVector = DrawRayUntilCollision();
                _endLineVector.z = _hitVector.z > _distance ? _distance : _hitVector.z;
                _lineRenderer.SetPosition(1, _endLineVector);
            }
        }


        private bool FindLaserIfExists(GameObject weaponObject)
        {
            _isLaserExist = false;
            _lineRenderer = weaponObject.GetComponentInChildren<LineRenderer>();

            if (_lineRenderer != null)
                _isLaserExist = true;

            return _isLaserExist;
        }


        private void InitLaserObject()
        {
            _laserObject = _lineRenderer.transform.gameObject;
            _laserTransform = _laserObject.transform;
            _distance = _lineRenderer.GetPosition(1).z;
            _endLineVectorDefault = _lineRenderer.GetPosition(1); 
            _endLineVector = _lineRenderer.GetPosition(1);
        }
        

        private Vector3 DrawRayUntilCollision()
        {
            _hitVector = _endLineVectorDefault;
            
            var ray = new Ray(_laserTransform.position, _laserTransform.forward);

            RaycastHit[] hits = Physics.RaycastAll(ray, _distance);

            if (hits.Length > 0)
            {
                bool isFoundCollider = hits
                    .Select(hit => !hit.collider.isTrigger)
                    .First();
                
                if (isFoundCollider)
                {
                    var closestHit = hits.Where(h => !h.collider.isTrigger)
                        .OrderBy(h => Vector3.Distance(h.point, _laserTransform.position))
                        .First();
                    
                    _hitVector = _laserTransform.InverseTransformPoint(closestHit.point);
                }
            }
            return _hitVector;
        }

        
        public void Blink(float time)
        {
            _laserObject.SetActive(false);
            Observable
                .Timer(TimeSpan.FromSeconds(time))
                .Subscribe(_ => _laserObject.SetActive(true));
        }


        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
        }
        
        
    }
}