using UnityEngine;
using Core.Models;
using Core.Generation;
 

namespace Core
{
    public class World : MonoBehaviour
    {
        private Transform _playerTransform;
        [SerializeField] private Material _material;
        [SerializeField] private TextureDataConfig _textureConfig;

        
        private Generator _generator;

        
        public void Init(Transform playerTransform)
        {
            _playerTransform = playerTransform;
            WorldGeneration.SetRandomSid();
            WorldChunckObjects worldChunckObjects = new WorldChunckObjects();

            _generator = new Generator(playerTransform, _material, this.transform, this,_textureConfig);
 
            _generator.WorldSetUp(worldChunckObjects, true);
             
        }


        private void Update()
        {
            if(_playerTransform != null)
                _generator.RunGenerator();
            
        }


    }
}
