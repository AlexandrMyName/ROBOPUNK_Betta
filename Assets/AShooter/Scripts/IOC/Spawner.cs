using UnityEngine;
using Zenject;

namespace DI.Spawn
{
    public class Spawner : MonoBehaviour
    {
        [Inject] private DiContainer _container;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _spawnTransform;

        public GameObject Spawn()
        {
            GameObject sceneInstance = _container.InstantiatePrefab(_prefab);
            sceneInstance.transform.position = _spawnTransform.position;
            return sceneInstance;
        }
    }
}
