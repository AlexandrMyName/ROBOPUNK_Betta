using System;
using AShooter.Scripts.IOC;
using Cinemachine;
using Core;
using UnityEngine;
using Zenject;


namespace DI.Spawn
{
    
    public class Spawner : MonoBehaviour
    {
        
        [Inject] private DiContainer _container;
        [Inject] private SpawnPlayerFactory _spawnPlayerFactory;
        [Inject] private CinemachineVirtualCamera _camera;
        
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _spawnTransform;
        

        public GameObject Prefab => _prefab;


        // private void Awake()
        // {
        //     Player player = _spawnPlayerFactory.Create();
        //     
        //     _camera.Follow = player.transform;
        //     _camera.LookAt = player.transform;
        //
        //     _container.Bind<Transform>().WithId("PlayerTransform").FromInstance(player.transform).AsCached();
        //     _container.Bind<Player>().FromInstance(player).AsCached();
        // }
        
        
    }
}
