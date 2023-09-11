using Core.Models;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Core.Generation
{
    public static class WorldGeneration
    {
        private static int _width = 25;
        private static int _height = 128;


        public static int Width => _width;
        public static int Height => _height;
        public static float Scale => 0.25f;

        static int heihtSid;
        static float sidNoise;
        public static void SetChunckGlobalSettings(int width, int height)
        {
            _width = width;
            _height = height;

        }
        public static void SetRandomSid(bool isIgnored = false)
        {
                heihtSid = Random.Range(2, 7);
                sidNoise = Random.Range(0.1511f, 0.3000f);
            
        }
        public static BlockType[,,] GetChunckTerrain(float xOffSet, float zOffSet,int stoneHeight)
        {
            BlockType[,,] blocks = new BlockType[Width, Height, Width];
 

            heihtSid = 5;

            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Width; z++)
                {

                    float height = Mathf.PerlinNoise((x / 4f + xOffSet) * sidNoise, (z / 4f + zOffSet) * sidNoise) * 25f + heihtSid;

                    for (int y = 0; y < height; y++)
                    {
                        if (y == 0)
                        {
                            blocks[x, y, z] = BlockType.Bedrock;
                        }
                        else if (height - y  > stoneHeight)
                        {
                            blocks[x, y, z] = BlockType.Stone;
                        }
                        else
                        {
                            blocks[x, y, z] = BlockType.Grass;
                        }
                    }

                }
            }

            return blocks;
        }
    }
}