using System.Collections.Generic;
using UnityEngine;

namespace Core.Models
{
    public class MeshData
    {
        public List<Vector3> Verticals;
        public List<int> Triangles;
        public List<Vector2> Uvs;
        public Vector3 WorldPositionStay;
        public Vector2Int ChunckPosition;




        public void SetVertexBufferData(List<Vector3> verticals) => Verticals = verticals;
        public void SetTriangleBufferData(List<int> triangles) => Triangles = triangles;
        public void SetUVSBufferData(List<Vector2> uvs) => Uvs = uvs;

        public void SetWorldPosBufferData(Vector3 worldPosition) => WorldPositionStay = worldPosition;


    }
}