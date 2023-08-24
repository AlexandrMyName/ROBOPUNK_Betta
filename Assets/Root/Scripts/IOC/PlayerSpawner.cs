using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DI.Spawn
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Inject] private DiContainer _container;
        [SerializeField] private GameObject _playerPrefab;



        public GameObject Spawn(Vector3 spawnPos)
        {


            GameObject sceneInstance = _container.InstantiatePrefab(_playerPrefab);
            sceneInstance.transform.position = spawnPos;
            return sceneInstance;
        }
    }
}
