using System.Collections.Generic;
using Core.Models;
using UnityEngine;

namespace Core.Generation
{
    public class ChunckRenderer
    {
        private List<Vector3> _verticals;
        private List<int> _triangles;
       
        private TextureRenderer _textureRender;
        private ChunckData _data;
 
        public ChunckRenderer(TextureDataConfig textureConfig)
        {
            _textureRender = new TextureRenderer(textureConfig);



        }
        public MeshData Generate(ChunckData chunckData)
        {
            CreateChunck(chunckData);
            MeshData meshData = new();
            meshData.ChunckPosition = chunckData.ChunckPosition;
            meshData.SetTriangleBufferData(_triangles);
            meshData.SetVertexBufferData(_verticals);
            meshData.SetUVSBufferData(_textureRender.GetUVs());
            meshData.SetWorldPosBufferData(_data.WorldPosition);
            return meshData;
        }
        public MeshData SetBlock()
        {
            if (_data == null) return null;
 
            Generate();
            MeshData meshData = new();
            meshData.SetTriangleBufferData(_triangles);
            meshData.SetVertexBufferData(_verticals);
            meshData.SetUVSBufferData(_textureRender.GetUVs());
            meshData.SetWorldPosBufferData(_data.WorldPosition);
            return meshData;

        }
        private void CreateChunck(ChunckData data)
        {
            _data = data;

            Generate();


        }
        private void Generate()
        {
            Init();

            for (int y = 0; y < WorldGeneration.Height; y++)
            {
                for (int x = 0; x < WorldGeneration.Width; x++)
                {
                    for (int z = 0; z < WorldGeneration.Width; z++)
                    {
                        CreateBlock(x, y, z);
                    }
                }
            }
            _data.Render = this;
        }
        private void Init()
        {
            _textureRender.Dispose();
            _verticals ??= new List<Vector3>();
            _triangles ??= new List<int>();
            _verticals.Clear();
            _triangles.Clear();
        }


        private void CreateBlock(int x, int y, int z)
        {
            Vector3Int blockPos = new Vector3Int(x, y, z);
            if (GetBlockAtPosition(blockPos) == 0) return; //?

            if (GetBlockAtPosition(blockPos + Vector3Int.right) == 0) CreateRightSide(blockPos);
            if (GetBlockAtPosition(blockPos + Vector3Int.left) == 0) CreateLeftSide(blockPos);
            if (GetBlockAtPosition(blockPos + Vector3Int.forward) == 0) CreateFrontSide(blockPos);
            if (GetBlockAtPosition(blockPos + Vector3Int.back) == 0) CreateBackSide(blockPos);
            if (GetBlockAtPosition(blockPos + Vector3Int.up) == 0) CreateTopSide(blockPos);
            if (GetBlockAtPosition(blockPos + Vector3Int.down) == 0) CreateDownSide(blockPos);

        }
        #region Sides
        private void CreateRightSide(Vector3Int blockPos)
        {
            _textureRender.AddTexture(GetBlockAtPosition(blockPos), false, SideData.Right);
            _verticals.Add((new Vector3(1, 0, 0) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(1, 1, 0) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(1, 0, 1) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(1, 1, 1) + blockPos) * WorldGeneration.Scale);
            AddLastSquereVerticals();

        }
        private void CreateLeftSide(Vector3Int blockPos)
        {
            _textureRender.AddTexture(GetBlockAtPosition(blockPos), false, SideData.Left);
            _verticals.Add((new Vector3(0, 0, 0) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(0, 0, 1) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(0, 1, 0) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(0, 1, 1) + blockPos) * WorldGeneration.Scale);
            AddLastSquereVerticals();
        }
        private void CreateFrontSide(Vector3Int blockPos)
        {
            _textureRender.AddTexture(GetBlockAtPosition(blockPos), false, SideData.Front);
            _verticals.Add((new Vector3(0, 0, 1) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(1, 0, 1) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(0, 1, 1) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(1, 1, 1) + blockPos) * WorldGeneration.Scale);
            AddLastSquereVerticals();

        }
        private void CreateBackSide(Vector3Int blockPos)
        {
            _textureRender.AddTexture(GetBlockAtPosition(blockPos), false, SideData.Back);
            _verticals.Add((new Vector3(0, 0, 0) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(0, 1, 0) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(1, 0, 0) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(1, 1, 0) + blockPos) * WorldGeneration.Scale);
            AddLastSquereVerticals();
        }
        private void CreateTopSide(Vector3Int blockPos)
        {
            _textureRender.AddTexture(GetBlockAtPosition(blockPos), true, SideData.Top);
            _verticals.Add((new Vector3(0, 1, 0) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(0, 1, 1) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(1, 1, 0) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(1, 1, 1) + blockPos) * WorldGeneration.Scale);
            AddLastSquereVerticals();
        }
        private void CreateDownSide(Vector3Int blockPos)
        {
            _textureRender.AddTexture(GetBlockAtPosition(blockPos), false, SideData.Down);
            _verticals.Add((new Vector3(0, 0, 0) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(1, 0, 0) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(0, 0, 1) + blockPos) * WorldGeneration.Scale);
            _verticals.Add((new Vector3(1, 0, 1) + blockPos) * WorldGeneration.Scale);
            AddLastSquereVerticals();
        }
        #endregion
        private BlockType GetBlockAtPosition(Vector3Int blockPos)
        {
            if (blockPos.x >= 0 && blockPos.y >= 0 && blockPos.z >= 0
                && blockPos.x < WorldGeneration.Width && blockPos.y < WorldGeneration.Height && blockPos.z < WorldGeneration.Width
                 )
            {

                return _data.Blocks[blockPos.x, blockPos.y, blockPos.z];
            }
            else
            {
                return BlockType.Air;
            }
        }


        private void AddLastSquereVerticals()
        {
 
            _triangles.Add(_verticals.Count - 4);
            _triangles.Add(_verticals.Count - 3);
            _triangles.Add(_verticals.Count - 2);

            _triangles.Add(_verticals.Count - 3);
            _triangles.Add(_verticals.Count - 1);
            _triangles.Add(_verticals.Count - 2);
        }



    }
}