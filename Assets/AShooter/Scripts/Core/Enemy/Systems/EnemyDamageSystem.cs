using abstracts;
using UnityEngine;

namespace Core
{
    public class EnemyDamageSystem : BaseSystem
    {
        private IGameComponents _components;

        protected override void Awake(IGameComponents components)
        {
            _components = components;
        }

        protected override void Update()
        {

        }
    }
}


