using System;
using UnityEngine;


namespace Core
{
    
    internal sealed class PCInputHorizontal : IUserInputProxy
    {
        
        public event Action<float> AxisOnChange = delegate(float f) {  };
        public event Action<bool> ButtonAxisOnChange;

        
        public void GetAxis()
        {
            AxisOnChange.Invoke(Input.GetAxis(AxisManager.HORIZONTAL));
        }
        
        
    }
}