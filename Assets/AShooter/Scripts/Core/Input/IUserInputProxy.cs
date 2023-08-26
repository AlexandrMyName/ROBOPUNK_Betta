using System;


namespace Core
{
    
    public interface IUserInputProxy
    {
        
        public event Action<float> AxisOnChange;
        public event Action<bool> ButtonAxisOnChange;
        
        public void GetAxis();
        
    }
}