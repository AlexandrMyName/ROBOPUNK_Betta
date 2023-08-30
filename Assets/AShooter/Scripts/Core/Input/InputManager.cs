 
public static class InputManager  
{
    private static InputConfig _config;
    public static void InitSystem(InputConfig cnf) => _config = cnf;

    public static void EnableSystem() => _config?.Enable();
       
    public static void DisableSystem() => _config?.Disable();   
}
