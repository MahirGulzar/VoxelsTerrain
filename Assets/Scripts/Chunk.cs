using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chunk : MonoBehaviour
{

    public int width;

    private Color[,,] chunkData;

    public UnityEvent OnBlockUpdate;

    private void Awake()
    {
        chunkData = new Color[width, width, width];
    }

    internal void SetBlock(int x, int y, int z, Color blockData)
    {
        chunkData[x, y, z] = blockData;
        //SendMessage("BlockUpdate", this);

        
        OnBlockUpdate.Invoke();
    }

    public void FillBlocks(Color[,,] blockData)
    {
        for(int z =0;z<width;++z)
            for (int y = 0; y < width; ++y)
                for (int x = 0; x < width; ++x)
                {
                    chunkData[x, y, z] = blockData[x, y, z];
                }

        OnBlockUpdate.Invoke();
    }

    internal Color GetBlock(int x, int y, int z)
    {
        return chunkData[x, y, z];
    }
}