using Abstracts;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{

    public class EnemyMovementSystem : BaseSystem
    {

        private NavMeshAgent _navMeshAgent;
        private Transform _targetTransform;

        protected override void Awake(IGameComponents components)
        {
            _targetTransform = components.BaseTransform.GetComponent<Enemy>().playerTransform;

            _navMeshAgent = components.BaseObject.GetComponent<NavMeshAgent>();
            if (_navMeshAgent == null)
            {
                Debug.LogError($"NavMeshAgent not found on Enemy object - {components.BaseObject.name}");
            }
        }


        protected override void OnEnable()
        {
            Debug.LogWarning("Update");
            _navMeshAgent.ResetPath();
        }


        protected override void Update() => Moving(_targetTransform.position);
        

        public void Moving(Vector3 targetPosition)
        {
            if (_navMeshAgent.isActiveAndEnabled)
                _navMeshAgent.SetDestination(targetPosition);
        }


    }

}