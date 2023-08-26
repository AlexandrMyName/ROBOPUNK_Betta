using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Core;
using abstracts;
using DI.Spawn;
using Cinemachine;


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
 
