
namespace Core
{
    
    public sealed class PCInput : IInput
    {
        
        public IUserInputProxy Horizontal { get; }
        public IUserInputProxy Vertical { get; }
        public IUserInputProxy LeftClick { get; }

        
        public PCInput()
        {
            Horizontal = new PCInputHorizontal();
            Vertical = new PCInputVertical();
            LeftClick = new PCAttackInput();
        }
        
        
    }
}