using Core.Generation;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Models
{

    public class WorldChunckObjects
    {
        public Dictionary<Vector2Int, ChunckData> ChunckData = new Dictionary<Vector2Int, ChunckData>();

        public Dictionary<Vector2Int, GameObject> Chuncks = new Dictionary<Vector2Int, GameObject>();


        public Vector2Int GetChunckContaineBlock(Vector3Int worldPos)
        {
            
            Vector2Int chunckPos = Vector2Int.FloorToInt(new Vector2(worldPos.x * 1f/ WorldGeneration.Width, worldPos.z * 1f/WorldGeneration.Width));
    
            return chunckPos;
        }
        public GameObject GetChunck(Vector2Int worldPos)
        {
            if (Chuncks.TryGetValue(worldPos, out var gm))
            {
                return gm;
            }
            return null;
        }

    }
}