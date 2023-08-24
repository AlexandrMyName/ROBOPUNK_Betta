using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Core;
using abstracts;
using DI.Spawn;

namespace DI
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private bool _spawnOnAwake;
        [SerializeField] private PlayerSpawner _spawner;

        [Space(10), SerializeField] private bool _useMoveSystem;
        [SerializeField] private bool _useShootSystem;

        private ObjectStack _componentStack;

        public override void InstallBindings()
        {

            List<ISystem> _systems = new List<ISystem>();

            if (_useMoveSystem)
                _systems.Add(new MoveSystem());

            if (_useShootSystem)
                _systems.Add(new ShootSystem());

            Container.Bind<List<ISystem>>().FromInstance(_systems).AsCached();
        }
        private void Awake()
        {

            if (_spawnOnAwake)
                _spawner.Spawn(Vector3.zero + Vector3.up);
        }
    }
}
 
