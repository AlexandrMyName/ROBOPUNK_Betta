using Zenject;
using Core;
using Abstracts;


namespace DI
{
    
    public class InputInstaller : MonoInstaller
    {
        
        public override void InstallBindings() => Container
            .Bind<IInput>()
            .FromInstance(new PCInput())
            .AsCached();
        
    }
}
 
