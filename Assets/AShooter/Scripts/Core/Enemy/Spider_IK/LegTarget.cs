using UnityEngine;


namespace Core
{


    public class LegTarget : MonoBehaviour
    {

        [SerializeField] private float _stepSpeed;
        [SerializeField] private AnimationCurve _stepCurve;

        private Transform _transform;
        private Vector3 _position;
        private Movement? _movement;

        public Vector3 Position => _position;

        public bool IsMoving => _movement != null;


        public void MoveTo(Vector3 target)
        {

            if (_movement == null)
            {
                _movement = new Movement
                {
                    Progress = 0,
                    FromTo = _position,
                    To = target
                };
            }
            else
            {
                _movement = new Movement
                {
                    Progress = _movement.Value.Progress,
                    FromTo = _movement.Value.FromTo,
                    To = target
                };
            }
        }

        private struct Movement
        {

            public float Progress;
            public Vector3 FromTo;
            public Vector3 To;

            public Vector3 Evaluate(in Vector3 up, AnimationCurve animationCurve)
            {
                return Vector3.Lerp(FromTo, To, Progress) + up * animationCurve.Evaluate(Progress);
            }
        }


        private void Start()
        {

            _transform = base.transform;
            _position = transform.position;
        }


        private void Update()
        {

            if (_movement != null)
            {
                var m = _movement.Value;

                m.Progress = Mathf.Clamp01(m.Progress + Time.deltaTime * _stepSpeed);
                _position = m.Evaluate(Vector3.up, _stepCurve);

                _movement = m.Progress < 1 ? m : null;
            }

            _transform.position = _position;
        }


    }
}