using Abstracts;
using UniRx;
using UnityEngine;
using Zenject;


namespace Core.Components
{
    
    public sealed class MoveComponent
    {

        [Inject(Id = "PlayerSpeed")] public ReactiveProperty<float> Speed { get; }
        
        private Rigidbody _rigidbody;


        public MoveComponent(Rigidbody rigidbody) => _rigidbody = rigidbody;
        
        
        public void Move(Vector3 direction)
        {
            _rigidbody.velocity = direction * Speed.Value;
        }
        
        
    }
}