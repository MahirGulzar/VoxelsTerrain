using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CreateMesh : MonoBehaviour
{
    public int TileWidth = 16;
    public int TileHeight = 16;
    public int NumTilesX = 16;
    public int NumTilesY = 16;
    public int TileGridWidth = 100;
    public int TileGridHeight = 100;
    public int DefaultTileX;
    public int DefaultTileY;
    public Texture2D Texture;

    public Renderer renderer;

    private Texture2D heigtmapTexture;

    MeshFilter MeshFilter;
    void OnEnable()
    {
        renderer.material.mainTexture = pixelTexture();
        heigtmapTexture = (Texture2D)renderer.material.mainTexture;
        CreatePlane(TileWidth, TileHeight, TileGridWidth, TileGridHeight);
        MeshFilter = GetComponent<MeshFilter>();

    }

    void Update()
    {
        //var tileColumn = Random.Range(0, NumTilesX);
        //var tileRow = Random.Range(0, NumTilesY);

        //var x = Random.Range(0, TileGridWidth);
        //var y = Random.Range(0, TileGridHeight);

        //UpdateGrid(new Vector2(x, y), new Vector2(tileColumn, tileRow), TileWidth, TileHeight, TileGridWidth);


        Vector3[] verts = GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] DeformedVertices = new Vector3[GetComponent<MeshFilter>().mesh.vertices.Length];

        for (int i = 0; i < DeformedVertices.Length; i++)
        {
            DeformedVertices[i] = new Vector3(verts[i].x, Random.Range(0, 2), verts[i].z);
        }

        GetComponent<MeshFilter>().mesh.vertices = DeformedVertices;
        GetComponent<MeshFilter>().mesh.RecalculateNormals();


    }

    public void UpdateGrid(Vector2 gridIndex, Vector2 tileIndex, int tileWidth, int tileHeight, int gridWidth)
    {
        var mesh = GetComponent<MeshFilter>().mesh;
        var uvs = mesh.uv;


        // Changes here..
        var tileSizeX = 1.0f ;
        var tileSizeY = 1.0f;

        mesh.uv = uvs;

        uvs[(int)(gridWidth * gridIndex.x + gridIndex.y) * 4 + 0] = new Vector2(tileIndex.x * tileSizeX, tileIndex.y * tileSizeY);
        uvs[(int)(gridWidth * gridIndex.x + gridIndex.y) * 4 + 1] = new Vector2((tileIndex.x + 1) * tileSizeX, tileIndex.y * tileSizeY);
        uvs[(int)(gridWidth * gridIndex.x + gridIndex.y) * 4 + 2] = new Vector2((tileIndex.x + 1) * tileSizeX, (tileIndex.y + 1) * tileSizeY);
        uvs[(int)(gridWidth * gridIndex.x + gridIndex.y) * 4 + 3] = new Vector2(tileIndex.x * tileSizeX, (tileIndex.y + 1) * tileSizeY);

        mesh.uv = uvs;
    }

    void CreatePlane(int tileHeight, int tileWidth, int gridHeight, int gridWidth)
    {
        var mesh = new Mesh();
        var mf = GetComponent<MeshFilter>();
        mf.GetComponent<Renderer>().material.SetTexture("_MainTex", Texture);
        mf.mesh = mesh;


        // Changes here..
        var tileSizeX = 1.0f;
        var tileSizeY = 1.0f;

        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();

        var index = 0;
        for (var x = 0; x < gridWidth; x++)
        {
            for (var y = 0; y < gridHeight; y++)
            {
                //AddVertices(tileHeight, tileWidth, y, x, vertices);
                AddVertices(1, 1, y, x, vertices);
                index = AddTriangles(index, triangles);
                AddNormals(normals);
                AddUvs(DefaultTileX, tileSizeY, tileSizeX, uvs, DefaultTileY);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }

    private static void AddVertices(int tileHeight, int tileWidth, int y, int x, ICollection<Vector3> vertices)
    {

        int rand = Random.Range(0, 2);

        vertices.Add(new Vector3((x * tileWidth), 0,(y * tileHeight)));
        vertices.Add(new Vector3((x * tileWidth) + tileWidth,0,(y * tileHeight)));
        vertices.Add(new Vector3((x * tileWidth) + tileWidth, 0,(y * tileHeight) + tileHeight));
        vertices.Add(new Vector3((x * tileWidth), 0,(y * tileHeight) + tileHeight));
    }

    private static int AddTriangles(int index, ICollection<int> triangles)
    {
        triangles.Add(index + 2);
        triangles.Add(index + 1);
        triangles.Add(index);
        triangles.Add(index);
        triangles.Add(index + 3);
        triangles.Add(index + 2);
        index += 4;
        return index;
    }

    private static void AddNormals(ICollection<Vector3> normals)
    {
        normals.Add(Vector3.forward);
        normals.Add(Vector3.forward);
        normals.Add(Vector3.forward);
        normals.Add(Vector3.forward);
    }

    private static void AddUvs(int tileRow, float tileSizeY, float tileSizeX, ICollection<Vector2> uvs, int tileColumn)
    {
        uvs.Add(new Vector2(tileColumn * tileSizeX, tileRow * tileSizeY));
        uvs.Add(new Vector2((tileColumn + 1) * tileSizeX, tileRow * tileSizeY));
        uvs.Add(new Vector2((tileColumn + 1) * tileSizeX, (tileRow + 1) * tileSizeY));
        uvs.Add(new Vector2(tileColumn * tileSizeX, (tileRow + 1) * tileSizeY));
    }


    public static Texture2D pixelTexture()
    {
        Texture2D generatedTexture = new Texture2D(100, 100);
        for(int i=0;i<100; i++)
        {
            for(int j=0;j<100;j++)
            {

                Color color;

                int rand = Random.Range(0, 2);
                if(rand==0)
                {
                    color = Color.black;
                }
                else
                {
                    color = Color.white;
                }
                generatedTexture.SetPixel(i, j, color);
            }
        }

        generatedTexture.wrapMode = TextureWrapMode.Clamp;
        generatedTexture.filterMode = FilterMode.Point;
        generatedTexture.Apply();

        return generatedTexture;
    }
}


