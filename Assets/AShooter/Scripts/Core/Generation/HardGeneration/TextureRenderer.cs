using Core.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using static TextureDataConfig;

public class TextureRenderer  : IDisposable
{
   
    float width = 16f;
    float height = 16f;

    private TextureDataConfig _config;
    private List<Vector2> _uvs;

    public TextureRenderer(TextureDataConfig config)
    {
       
        _config = config;
        _uvs = new List<Vector2>();
    }
    public List<Vector2> GetUVs() => _uvs;

    public void AddTexture(BlockType block,bool isTop, SideData sideType)
    {
       Vector2 textureUV =  GetTexture(block, isTop);

        float x0 = textureUV.x - 1 > 0 ? (textureUV.x - 1) / width : 0.0f;
        float x1 = textureUV.x > 0 ? textureUV.x / width : 1f;

        float y0 = textureUV.y - 1 > 0 ? (textureUV.y - 1) / height : 0.0f;
        float y1 = textureUV.y > 0 ? textureUV.y / height : 1f;

        if (sideType == SideData.Left || sideType == SideData.Front || sideType == SideData.Top || sideType == SideData.Down)
        {
            _uvs.Add(new Vector2(x0, y0));
            _uvs.Add(new Vector2(x1, y0));
            _uvs.Add(new Vector2(x0, y1));
            _uvs.Add(new Vector2(x1, y1));
        }
        else
        {
            _uvs.Add(new Vector2(x0, y0));
            _uvs.Add(new Vector2(x0, y1));
            _uvs.Add(new Vector2(x1, y0));
            _uvs.Add(new Vector2(x1, y1));
        }
 
    }
   
    private Vector2 GetTexture(BlockType block,bool isTop = false)
    {
        Vector2 textureUV = Vector2.zero;
        TextureConfig config = _config.Configs.Find(conf => conf.BlockType == block);

        switch (block)
        {
            case BlockType.Air:


                break;
            case BlockType.Grass:

                

                if (isTop)
                {
                    textureUV = config.TopSide;
                }
                else
                {
                    textureUV = config.RightSide;
                }

                break;

            case BlockType.Stone:

                textureUV = config.RightSide;

                break;

            case BlockType.Bedrock:

                textureUV = config.RightSide;

                break;

            case BlockType.Wood:

                textureUV = config.RightSide;

                break;

            case BlockType.WoodBoards:

                textureUV = config.RightSide;

                break;

            case BlockType.Break:

                textureUV = config.RightSide;

                break;
        }
        return textureUV;
    }

    public void Dispose()
    {
        _uvs ??= new();
        _uvs.Clear();
    }
}
