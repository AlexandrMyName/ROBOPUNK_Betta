using abstracts;
using UnityEngine;

namespace Core
{
    public class EnemyMovementSystem : BaseSystem
    {
        private IGameComponents _components;
        
        protected override void Awake(IGameComponents components)
        {
            _components = components;
            Debug.Log($"Initialized move system! ({components.BaseObject.name})");
            
        }

        protected override void Update()
        {
            
        }
    }
}