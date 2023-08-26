using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Core;
using abstracts;
using DI.Spawn;
using Cinemachine;

namespace DI
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private bool _spawnOnAwake;
        [SerializeField] private Spawner _spawner;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [Space(10), SerializeField] private bool _useMoveSystem;
        [SerializeField] private bool _useShootSystem;


        public override void InstallBindings()
        => Container.Bind<List<ISystem>>().WithId("PlayerSystems").FromInstance(InitSystem()).AsCached();
        
        private List<ISystem> InitSystem()
        {
            List<ISystem> systems = new List<ISystem>();

            if (_useMoveSystem)
                systems.Add(new PlayerMovement());

            if (_useShootSystem)
                systems.Add(new PlayerShootSystem());

            return systems;
        }

        private void Awake()
        {
            
            if (_spawnOnAwake)
            {
               GameObject player = _spawner.Spawn();
                _camera.Follow = player.transform;
                _camera.LookAt = player.transform;
            }
        }
    }
}
 
