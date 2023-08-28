using Abstracts;
using UnityEngine;


namespace Core
{
    
    public sealed class PlayerShootSystem : BaseSystem
    {
        
        private IGameComponents _components;
        
        
        protected override void Awake(IGameComponents components)
        {
            _components = components;
            Debug.Log($"Initialized shoot system! ({components.BaseObject.name})");
        }
        
        
    }
}
