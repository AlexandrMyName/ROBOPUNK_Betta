using abstracts;
using UniRx;
using UnityEngine;

namespace Core
{
    public class EnemyDamageSystem : BaseSystem
    {
        private IGameComponents _components;
    
        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _components.BaseObject.GetComponent<IAttackable>().Health.Subscribe(OnDamage);
        }

        private void OnDamage(float amountHealth)
        {
            Debug.LogWarning($"{_components.BaseObject.name} getting damage |DamageSystem|");
        }
    }
}


