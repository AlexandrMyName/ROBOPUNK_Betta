using UnityEngine;


namespace Core
{

    public class LegRaycast : MonoBehaviour
    {

        [SerializeField] private LayerMask _layersForIK;
        [SerializeField] private float Length = 3f;
        private Transform _transform;
        private RaycastHit _hit;

        public Vector3 Position => _hit.point;
        public Vector3 Normal => _hit.normal;

        public Vector3 NormalLeg;


        private void Start()
        {

            _transform = base.transform;
        }


        private void Update()
        {

            Ray ray = new Ray(_transform.position, -_transform.up);
            Physics.Raycast(ray, out _hit, Length, _layersForIK);
        }


    }
}
