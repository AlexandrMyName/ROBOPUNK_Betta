using abstracts;
using User;
using UnityEngine;

namespace Core
{
    public class EnemyDamageSystem : BaseSystem
    {
        private IGameComponents _components;
        private IHealth _health;

        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _health = _components.BaseObject.GetComponent<IHealth>();
        }

        protected override void Update()
        {

            if (_health != null && _health.CurrentHealth <= 0)
            {
                Debug.LogWarning($"Death of the enemy  {_components.BaseObject.name}");
                Object.Destroy(_components.BaseObject);
            }
        }
    }
}


