using Abstracts;
using UnityEngine;


namespace Core
{
    
    public class ObjectStack : IGameComponents
    {
        
        private Camera _camera;
        private Transform _transform;
        private GameObject _gameObject;
        private IAnimatorIK _animatorIK;


        public ObjectStack(Camera cam, GameObject baseObject, IAnimatorIK animatorIK)
        {

            _camera = cam;
            _gameObject = baseObject;
            _transform = baseObject.transform;
            _animatorIK = animatorIK;
        }
        
        
        public Camera MainCamera => _camera;

        public Transform BaseTransform => _transform;

        public GameObject BaseObject => _gameObject;

        public IAnimatorIK Animator => _animatorIK;
    }
}
