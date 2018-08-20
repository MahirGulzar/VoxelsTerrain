using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMesh : MonoBehaviour {

    void Update()
    {
        //var tileColumn = Random.Range(0, NumTilesX);
        //var tileRow = Random.Range(0, NumTilesY);

        //var x = Random.Range(0, TileGridWidth);
        //var y = Random.Range(0, TileGridHeight);

        //UpdateGrid(new Vector2(x, y), new Vector2(tileColumn, tileRow), TileWidth, TileHeight, TileGridWidth);

        //CreatePlane(TileWidth, TileHeight, TileGridWidth, TileGridHeight);

        Vector3[] verts = GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] DeformedVertices = new Vector3[GetComponent<MeshFilter>().mesh.vertices.Length];

        for (int i = 0; i < DeformedVertices.Length; i++)
        {
            DeformedVertices[i] = new Vector3(verts[i].x, Random.Range(0, 2), verts[i].z );
        }

        GetComponent<MeshFilter>().mesh.vertices = DeformedVertices;
        GetComponent<MeshFilter>().mesh.RecalculateNormals();


    }

}
