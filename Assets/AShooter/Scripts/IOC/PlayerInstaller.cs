using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Core;
using abstracts;
using DI.Spawn;
using UnityEngine.Rendering.VirtualTexturing;

namespace DI
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private bool _spawnOnAwake;
        [SerializeField] private Spawner _spawner;
        [Space(10), SerializeField] private bool _useMoveSystem;
        [SerializeField] private bool _useShootSystem;


        public override void InstallBindings()
        => Container.Bind<List<ISystem>>().WithId("PlayerSystems").FromInstance(InitSystem()).AsCached();
        
        private List<ISystem> InitSystem()
        {
            List<ISystem> systems = new List<ISystem>();

            if (_useMoveSystem)
                systems.Add(new PlayerMovable());

            if (_useShootSystem)
                systems.Add(new PlayerShootable());

            return systems;
        }

        private void Awake()
        {

            if (_spawnOnAwake)
                _spawner.Spawn();
        }
    }
}
 
