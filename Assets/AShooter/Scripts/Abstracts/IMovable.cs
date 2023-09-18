using UniRx;
using UnityEngine;

namespace Abstracts
{
    
    public interface IMovable
    {

        ReactiveProperty<float> Speed { get; }

        void Move(Vector3 direction);

        void InitComponent(Rigidbody rb);
    }
}