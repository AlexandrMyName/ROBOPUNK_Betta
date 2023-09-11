using Core.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Generation
{
    public static class MeshBuilder
    {
        public static MeshData GenerateMeshData(ChunckRenderer render, ChunckData data)
        {
            return render.Generate(data);
        }
    }
}