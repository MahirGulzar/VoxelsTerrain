using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinChunkGenerator : MonoBehaviour {

    public int seed;
    private Chunk chunk;

	// Use this for initialization
	void Start () {
        chunk = GetComponent<Chunk>();

        Generate(chunk, seed);
	}

    private void Generate(Chunk chunk, int seed)
    {
        Random.InitState(seed);

        for(int x=0;x<chunk.width;x++)
            for (int y = 0; y < chunk.width; y++)
                for (int z = 0; z < chunk.width; z++)
                {
                    chunk.SetBlock(x, y, z, Color.yellow);
                }
    }

    // Update is called once per frame
    void Update () {
	}

}



