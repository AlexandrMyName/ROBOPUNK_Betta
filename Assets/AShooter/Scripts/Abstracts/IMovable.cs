using UniRx;
using UnityEngine;

namespace Abstracts
{
    public interface IMovable
    {
        public ReactiveProperty<float> Speed { get; }

        public void Move(Vector3 direction);
    }
}