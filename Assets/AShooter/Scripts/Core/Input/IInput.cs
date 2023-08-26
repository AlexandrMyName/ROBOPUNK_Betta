
namespace Core
{
    
    public interface IInput
    {
        
        IUserInputProxy Horizontal { get; }
        
        IUserInputProxy Vertical { get; }

        IUserInputProxy LeftClick { get; }
        

    }
}