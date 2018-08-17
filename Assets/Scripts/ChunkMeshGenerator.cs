﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Chunk))]
public class ChunkMeshGenerator : MonoBehaviour
{

    private MeshFilter filter;

    private void Awake()
    {
        var chunk = GetComponent<Chunk>();
        if(chunk==null)
        {
            throw new Exception("No chunk found for mesh generator");
        }
        chunk.OnBlockUpdate.AddListener(()=> BlockUpdate(chunk));
    }

    void BlockUpdate(Chunk updatedChunk)
    {
        Debug.Log("Updating Mesh");
        var generatedMesh = GenerateMesh(updatedChunk);
        if (filter == null)
        {
            filter = GetComponent<MeshFilter>();
        }
        filter.mesh = generatedMesh;
    }

    private Mesh GenerateMesh(Chunk updatedChunk)
    {
        Mesh mesh = new Mesh();
        var generatedColors = new List<Color>();
        var generatedVertices = new List<Vector3>();
        var generatedTriangles = new List<int>();

        for (var x = 0; x < updatedChunk.width; ++x)
        {
            for (var y = 0; y < updatedChunk.width; ++y)
            {
                for (var z = 0; z < updatedChunk.width; ++z)
                {
                    var block = CheckBlock(updatedChunk, x, y, z);
                    if (block.color != default(Color))
                    {
                        // current Block is valid
                        var adjacentBlocks = new Block[] {
                            CheckBlock(updatedChunk, x-1, y, z),
                            CheckBlock(updatedChunk, x+1, y, z),
                            CheckBlock(updatedChunk, x, y-1, z),
                            CheckBlock(updatedChunk, x, y+1, z),
                            CheckBlock(updatedChunk, x, y, z-1),
                            CheckBlock(updatedChunk, x, y, z+1)
                        };

                        GenerateMeshForAdjacentBlocks(block, adjacentBlocks, generatedColors, generatedTriangles, generatedVertices);
                    }
                }
            }
        }

        mesh.vertices = generatedVertices.ToArray();
        mesh.colors = generatedColors.ToArray();
        mesh.triangles = generatedTriangles.ToArray();

        return mesh;
    }

    private void GenerateMeshForAdjacentBlocks(Block block, Block[] adjacentBlocks, List<Color> generatedColors, List<int> generatedTriangles, List<Vector3> generatedVertices)
    {
        foreach (var adj in adjacentBlocks)
        {
            if (adj.color == default(Color))
            {
                Vector3 currentPosition = new Vector3(block.x, block.y, block.z);
                Vector3 adjPosition = new Vector3(adj.x, adj.y, adj.z);
                bool xDiff = block.x != adj.x;
                bool yDiff = block.y != adj.y;
                bool zDiff = block.z != adj.z;
                Vector3 u = Vector3.zero;
                Vector3 v = Vector3.zero;
                if (xDiff)
                {
                    u = Vector3.forward;
                    v = Vector3.up;
                }
                else if (yDiff)
                {
                    u = Vector3.right;
                    v = Vector3.forward;
                }
                else
                {
                    u = Vector3.right;
                    v = Vector3.up;
                }

                u *= 0.5f;
                v *= 0.5f;

                // Generate actual wall
                Vector3 quadPosition = (adjPosition + currentPosition) / 2.0f;
                int currentIndex = generatedVertices.Count;

                generatedVertices.Add(quadPosition + u + v);
                generatedVertices.Add(quadPosition + u - v);
                generatedVertices.Add(quadPosition - u + v);
                generatedVertices.Add(quadPosition - u - v);

                generatedColors.Add(block.color);
                generatedColors.Add(block.color);
                generatedColors.Add(block.color);
                generatedColors.Add(block.color);

                generatedTriangles.Add(currentIndex + 0);
                generatedTriangles.Add(currentIndex + 1);
                generatedTriangles.Add(currentIndex + 2);
                generatedTriangles.Add(currentIndex + 1);
                generatedTriangles.Add(currentIndex + 2);
                generatedTriangles.Add(currentIndex + 3);
            }
        }
    }

    private Block CheckBlock(Chunk updatedChunk, int x, int y, int z)
    {
        if (x < 0 || y < 0 || z < 0 ||
            x >= updatedChunk.width || y >= updatedChunk.width || z >= updatedChunk.width)
        {
            return new Block(x, y, z, default(Color));
        }
        return new Block(x, y, z, updatedChunk.GetBlock(x, y, z));
    }

    private class Block
    {
        public int x;
        public int y;
        public int z;
        public Color color;

        public Block(int x, int y, int z, Color color)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.color = color;
        }
    }
}