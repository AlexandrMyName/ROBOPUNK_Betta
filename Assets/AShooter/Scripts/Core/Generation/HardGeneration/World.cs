using UnityEngine;
using Core.Models;
using Core.Generation;

namespace Core
{
    public class World : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Material _material;
        [SerializeField] private TextureDataConfig _textureConfig;


        private Generator _generator;
        
        private void Awake() => WorldGeneration.SetRandomSid();


        private void Start()
        {
           
            WorldChunckObjects worldChunckObjects = new WorldChunckObjects();

            _generator = new Generator(_playerTransform, _material, this.transform, this,_textureConfig);

           

            _generator.WorldSetUp(worldChunckObjects, true);
             
        }


        private void Update()
        {
            
            _generator.RunGenerator();
            
        }


    }
}
