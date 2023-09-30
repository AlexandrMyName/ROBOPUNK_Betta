using Zenject;
using Core;
using Abstracts;
using UnityEngine;

namespace DI
{
    public class InputInstaller : MonoInstaller
    {
        private InputConfig _inputConfig;
        private IInteractable _interactable;

        public override void Start() => InputManager.EnableSystem();

        public override void InstallBindings()
        {
            GameObject chestObject = GameObject.FindGameObjectWithTag("Chest");
            _interactable = chestObject.GetComponent<IInteractable>();          
            Container.Bind<IInput>().FromInstance(InitInputSystem()).AsCached();
        }
 
        private PCInput InitInputSystem()
        {
            _inputConfig = new InputConfig();
            InputManager.InitSystem(_inputConfig);
            return new PCInput(_inputConfig, _interactable);
        }
    }
}
 
