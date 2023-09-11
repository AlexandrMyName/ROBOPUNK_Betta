using Core.Models;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Generation
{
    public class Generator
    {
        //Afonin A.I
        [SerializeField] private Material _chuncksMaterial;
        private Transform _playerTransform;
        private Transform _worldParrent;
        private int _radiusGenerate = 5;
        private Vector2Int _currentPlayerChunck;

        private ConcurrentQueue<MeshData> _meshDataQueue = new();

        private Camera _camera;
        private WorldChunckObjects _worldObjects;
        private MonoBehaviour _initializer;
        TextureDataConfig _textureConfig;

        public Generator(Transform playerTransform, Material blocksMaterial, Transform worldParrent, MonoBehaviour initializer, TextureDataConfig textureConfig)
        {
            playerTransform.position += Vector3.up * 10f;
            _chuncksMaterial = blocksMaterial;
            _playerTransform = playerTransform;
            _worldParrent = worldParrent;
            _initializer = initializer;
            _camera = Camera.main;
            _textureConfig = textureConfig;
        }
        public void WorldSetUp(WorldChunckObjects objects, bool isGenerateOne = true, int radiusGeneration = 5)
        {
            _worldObjects = objects;
            _radiusGenerate = radiusGeneration;
            _worldObjects.ChunckData = new Dictionary<Vector2Int, ChunckData>();
            if(_playerTransform != null)
                _initializer.StartCoroutine(SetPlayerPosition());
            if (isGenerateOne) Generate();
        }
        public void RunGenerator()
        {
            Vector3Int worldPos = Vector3Int.FloorToInt(_playerTransform.position / WorldGeneration.Scale);
            Vector2Int playerChunck = _worldObjects.GetChunckContaineBlock(worldPos);

            if (playerChunck != _currentPlayerChunck)
            {
                _currentPlayerChunck = playerChunck;
                Generate();
            }

            if (_meshDataQueue.TryDequeue(out MeshData meshData))
            {

                GameObject generationObject = new("Gen");
                generationObject.transform.SetParent(_worldParrent, true);

                generationObject.transform.position = meshData.WorldPositionStay;
                Mesh mesh = new Mesh();
                MeshFilter filter = generationObject.AddComponent<MeshFilter>();
                MeshRenderer render = generationObject.AddComponent<MeshRenderer>();
                MeshCollider collider = generationObject.AddComponent<MeshCollider>();

                render.material = _chuncksMaterial;
                filter.mesh = mesh;
                mesh.vertices = meshData.Verticals.ToArray();
                mesh.triangles = meshData.Triangles.ToArray();
                mesh.uv = meshData.Uvs.ToArray();

                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
                mesh.Optimize();

                collider.sharedMesh = mesh;
                _worldObjects.Chuncks.Add(meshData.ChunckPosition, generationObject);
            }
        }

        private void Generate()
        => _initializer.StartCoroutine(RuntimeGenerationProccess());


        private IEnumerator SetPlayerPosition()
        {
            WaitForSeconds seconds = new WaitForSeconds(4);
            yield return seconds;
            _playerTransform.GetComponent<Rigidbody>().isKinematic = false;
        }
        private IEnumerator RuntimeGenerationProccess()
        {
            WaitForSeconds seconds = new WaitForSeconds(0.02f);

            for (int x = _currentPlayerChunck.x - _radiusGenerate; x < _currentPlayerChunck.x + _radiusGenerate; x++)
            {
                for (int z = _currentPlayerChunck.y - _radiusGenerate; z < _currentPlayerChunck.y + _radiusGenerate; z++)
                {
                    float xOffset = x * WorldGeneration.Width * WorldGeneration.Scale;
                    float zOffset = z * WorldGeneration.Width * WorldGeneration.Scale;

                    Vector2Int worldPosition = new Vector2Int(x, z);
                  
                    if (_worldObjects.ChunckData.ContainsKey(worldPosition)) continue;

                    BlockType[,,] blocks = WorldGeneration.GetChunckTerrain(xOffset, zOffset, 5);

                    ChunckData data = new ChunckData(blocks, new Vector3(xOffset, 0, zOffset));
                    _worldObjects.ChunckData.Add(worldPosition, data);
                    data.ChunckPosition = worldPosition;

                    InstantiateRenderStream(data);
                    yield return seconds;
                }
            }
        }
        private void InstantiateRenderStream(ChunckData data)
        {
            Task.Factory.StartNew(() =>
            {
                ChunckRenderer render = new(_textureConfig);
                MeshData meshData = MeshBuilder.GenerateMeshData(render, data);

                _meshDataQueue.Enqueue(meshData);
            });
        }


    }
}