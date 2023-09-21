using UniRx;
using UnityEngine;


namespace Abstracts
{
    
    public interface IMovable
    {

        Rigidbody Rigidbody { get; }

        Vector3 MoveDirection { get; set; }

        ReactiveProperty<float> Speed { get; }

        void InitComponent(Rigidbody rb);
    }
}