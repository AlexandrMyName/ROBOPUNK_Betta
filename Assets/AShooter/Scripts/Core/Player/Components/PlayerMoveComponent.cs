using Abstracts;
using UniRx;
using UnityEngine;
using Zenject;


namespace Core.Components
{
    
    public sealed class PlayerMoveComponent : IMovable
    {

        public void InitComponent(Rigidbody rb)
        {
            Rigidbody = rb;
            MoveDirection = Vector3.zero;
        }


        [Inject(Id = "PlayerSpeed")] public ReactiveProperty<float> Speed { get; }
        
        public Rigidbody Rigidbody { get; private set; }
        
        public Vector3 MoveDirection { get; set; }


    }
}