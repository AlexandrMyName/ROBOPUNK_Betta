using UnityEngine;


namespace Abstracts
{
    
    public interface IInput
    {
        
        IUserInputProxy<float> Horizontal { get; }
        
        IUserInputProxy<float> Vertical { get; }

        IUserInputProxy<bool> LeftClick { get; }

        
        IUserInputProxy<Vector3> MousePosition { get; }


    }
}