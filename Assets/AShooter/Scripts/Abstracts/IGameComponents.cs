 
using UnityEngine;

namespace abstracts
{
    public interface IGameComponents
    {
        Camera MainCamera { get; }
        Transform BaseTransform { get; }
        GameObject BaseObject { get; }
    }
}
