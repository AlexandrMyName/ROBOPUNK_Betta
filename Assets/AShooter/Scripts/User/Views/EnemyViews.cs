using UnityEngine;
using Abstracts;


namespace User.View {

    public class EnemyViews : MonoBehaviour, IEnemyViews
    {

        private Camera _camera;
        public IEnemyHealthView Health { get ; set; }


        public void InitViews()
        {
            _camera = Camera.main;
            Health = GetComponentInChildren<IEnemyHealthView>();

            if (Health == null)
            {
                Debug.LogError("Health view is null");
            }
        }

       
        private void LateUpdate()
        {

            transform.LookAt(_camera.transform);
            Quaternion rotationToCam = transform.rotation;

            rotationToCam.y = 0;
            rotationToCam.z = 0;
            transform.rotation = rotationToCam;
        }
    }
}