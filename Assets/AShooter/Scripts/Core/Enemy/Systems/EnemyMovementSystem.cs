using Abstracts;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    public class EnemyMovementSystem : BaseSystem
    {
        private IGameComponents _components;
        private NavMeshAgent _navMeshAgent;
        private Transform _targetTransform;


        protected override void Awake(IGameComponents components)
        {
            _components = components;

            _targetTransform =  _components.BaseTransform.GetComponent<Enemy>().playerTransform;
            Debug.Log($"Initialized move system! ({components.BaseObject.name})");

            _navMeshAgent = _components.BaseObject.GetComponent<NavMeshAgent>();
            if (_navMeshAgent == null)
            {
                Debug.LogError("NavMeshAgent не найден на объекте Enemy.");
                return;
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
            {
                _navMeshAgent.SetDestination(targetPosition);
            }
            else
            {   
                Debug.LogError("NavMeshAgent не активирован или не найден на объекте Enemy.");
            }
        }
    }
}
