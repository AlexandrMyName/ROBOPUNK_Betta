using abstracts;
using UnityEngine;

namespace Core
{
    public class PlayerShootable : BaseSystem
    {
        private IGameComponents _components;
        protected override void Awake(IGameComponents components)
        {
            _components = components;
            Debug.Log($"Initialized shoot system! ({components.BaseObject.name})");
        }
    }
}
