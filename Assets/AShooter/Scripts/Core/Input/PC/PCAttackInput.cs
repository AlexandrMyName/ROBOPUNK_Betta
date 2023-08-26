using System;
using UnityEngine;


namespace Core
{

    internal sealed class PCAttackInput : IUserInputProxy
    {
        
        public event Action<bool> ButtonAxisOnChange = delegate(bool b) {  };
        public event Action<float> AxisOnChange;

        
        public void GetAxis()
        {
            ButtonAxisOnChange.Invoke(Input.GetButtonUp(AxisManager.FIRE1));
        }
        
        
    }
}