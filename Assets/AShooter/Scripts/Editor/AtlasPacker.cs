using System.Collections.Generic;
using UnityEngine;
using System.IO;

using UnityEditor;



public class AtlasPacker : EditorWindow
{

    int blockSize = 16;
    int atlasSizeInBlocks = 16;
    int atlasSize;

    Object[] rawTextures;
    List<Texture2D> sortedTexture = new List<Texture2D>();
    Texture2D atlas;
    
    [MenuItem("TopDownShooter/Atlas packer")]
    public static void ShowWindow()
    {
        GetWindow<AtlasPacker>();
    }


    private void OnGUI()
    {
        atlasSize = blockSize * atlasSizeInBlocks;

        GUILayout.Label("Cone texture atlas packer", EditorStyles.boldLabel);

        blockSize = EditorGUILayout.IntField("Block size", blockSize);
        atlasSizeInBlocks = EditorGUILayout.IntField("Atlas size (in blocks)", atlasSizeInBlocks);

        GUILayout.Label(atlas);


        if(GUILayout.Button("Load Texture"))
        {
            rawTextures = new Object[atlasSize];
            Debug.Log(rawTextures.Length + " objects");
            LoadTexture();

             
        }
        if (sortedTexture.Count > 0)
        {
            if (GUILayout.Button("Pack Texture"))
            {
                atlas = new Texture2D(atlasSize, atlasSize);

                atlas.SetPixels(PackAtlas());
                atlas.Apply();

                SaveAtlasToPNG(atlas);
            }

            if (GUILayout.Button("Clear Texture"))
            {
                atlas = new Texture2D(atlasSize, atlasSize);
               
            }

            
        }
        
        //Stream vs Factory pool

    }


    private void LoadTexture()
    {
        sortedTexture.Clear();
        rawTextures = Resources.LoadAll("AtlasPacker", typeof(Texture2D));

        int index = 0;

        foreach(Object texture in rawTextures)
        {

            Texture2D t = (Texture2D)texture;
            if(t.width == blockSize && t.height == blockSize)
                sortedTexture.Add(t);
            index++;
        }

        Debug.Log($"Atlas packer: sorted textures |{sortedTexture.Count} count|");
    }

    private Color [] PackAtlas()
    {
        
        Color[] pixels = new Color[atlasSize * atlasSize];

        for(int x = 0; x< atlasSize; x++){
            for (int y = 0; y < atlasSize; y++){

                int currentBlockX = x / blockSize;
                int currentBlockY = y / blockSize; 

                int currentIndex = currentBlockY * atlasSizeInBlocks + currentBlockX;
                int currentPixelX = x - (currentBlockX * blockSize);
                int currentPixelY = y - (currentBlockY * blockSize);
                Debug.Log(x + " atlas: " + atlasSize);
                if(currentIndex < sortedTexture.Count)
                {
                    pixels[(atlasSize - y - 1) * atlasSize + x] = sortedTexture[currentIndex].GetPixel(currentPixelX, blockSize - currentPixelY - 1);
                }
                else
                { 
                    pixels[(atlasSize - y - 1) * atlasSize + x] = new Color(0f, 0f, 0f, 0f);
                }
            }
        }
        return pixels;
    }

    private void SaveAtlasToPNG(Texture2D atlas)
    {
        byte[] bytes = atlas.EncodeToPNG();


        File.WriteAllBytes(Application.dataPath + "/dataPacker.png",bytes);



    }
}
