using Abstracts;
using UnityEngine;


namespace Core
{
    
    public sealed class PCInput : IInput
    {
        
        public IUserInputProxy<float> Horizontal { get; }
        public IUserInputProxy<float> Vertical { get; }
        public IUserInputProxy<bool> LeftClick { get; }
        
        public IUserInputProxy<Vector3> MousePosition { get; }


        public PCInput()
        {
            Horizontal = new PCInputHorizontal();
            Vertical = new PCInputVertical();
            LeftClick = new PCAttackInput();
            MousePosition = new PCMousePositionInput();
        }
        
        
    }
}