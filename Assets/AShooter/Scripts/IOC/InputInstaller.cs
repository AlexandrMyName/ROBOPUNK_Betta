using Zenject;
using Core;
using Abstracts;
using UnityEngine;

namespace DI
{
    public class InputInstaller : MonoInstaller
    {

        private InputConfig _inputConfig;

        public override void Start() => InputManager.EnableSystem();

        public override void InstallBindings(){
                    
            Container.Bind<IInput>().FromInstance(InitInputSystem()).AsCached();
        }
 
        private PCInput InitInputSystem(){

            _inputConfig = new InputConfig();
            InputManager.InitSystem(_inputConfig);
            return new PCInput(_inputConfig);
        }
    }
}
 
