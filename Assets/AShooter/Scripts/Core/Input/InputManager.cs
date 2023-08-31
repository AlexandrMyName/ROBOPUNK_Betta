 
public static class InputManager  
{
    private static InputConfig _config;
    public static void InitSystem(InputConfig config) => _config = config;

    public static void EnableSystem() => _config?.Enable();
       
    public static void DisableSystem() => _config?.Disable();   
}
