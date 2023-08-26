namespace Core
{
    
    internal class InputManager : IInput
    {
        
        private IUserInputProxy _horizontal;
        private IUserInputProxy _vertical;
        private IUserInputProxy _mainFire;
        private IUserInputProxy _submit;
        
        
        public IUserInputProxy Horizontal => _horizontal;
        public IUserInputProxy Vertical => _vertical;

        public IUserInputProxy LeftClick => _mainFire;

        public IUserInputProxy Submit => _submit;

        
        public InputManager(IUserInputProxy horizontalInput, IUserInputProxy verticalInput)
        {
            _horizontal = horizontalInput;
            _vertical = verticalInput;
        }

        
        public InputManager(
            IUserInputProxy horizontalInput, 
            IUserInputProxy verticalInput,
            IUserInputProxy mainFireInput,
            IUserInputProxy submit)
        {
            _horizontal = horizontalInput;
            _vertical = verticalInput;
            _mainFire = mainFireInput;
            _submit = submit;
        }

    }
}