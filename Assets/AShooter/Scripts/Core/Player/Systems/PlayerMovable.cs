using abstracts;
using UnityEngine;

namespace Core 
{
    public class PlayerMovable : BaseSystem
    {
        private IGameComponents _components;
        private PlayerAnimator _animator;
        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _animator = components.BaseObject.GetComponent<PlayerAnimator>();
            Debug.Log($"Initialized move system! ({components.BaseObject.name})");
            if (_animator == null) Debug.LogWarning($"Player animator not found on {components.BaseObject.name}");
        }

        protected override void Update()
        {

        }
       
    }
}