using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomTerrain : MonoBehaviour {


    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    public enum AnchorPoint
    {
        TopLeft,
        TopHalf,
        TopRight,
        RightHalf,
        BottomRight,
        BottomHalf,
        BottomLeft,
        LeftHalf,
        Center
    }

    public int widthSegments = 1;
    public int lengthSegments = 1;
    public float width = 1.0f;
    public float length = 1.0f;
    public Orientation orientation = Orientation.Horizontal;
    public AnchorPoint anchor = AnchorPoint.Center;
    public bool addCollider = false;
    public bool createAtOrigin = true;
    public bool twoSided = false;
    public string optionalName;

    public Renderer renderer;

    private Texture2D heigtmapTexture;


    private void Start()
    {
        renderer.material.mainTexture = pixelTexture();
        heigtmapTexture = (Texture2D)renderer.material.mainTexture;

        Texture2D newTexture=new Texture2D(heigtmapTexture.width, heigtmapTexture.height);
        Graphics.CopyTexture(heigtmapTexture, newTexture);

        Color[] pixels = newTexture.GetPixels();
        Array.Reverse(pixels);
        newTexture.SetPixels(pixels);
        //newTexture.Apply();

        //Color[,] height = new Color[heigtmapTexture.width, heigtmapTexture.height];
        //Color[,] newHeight= new Color[heigtmapTexture.width, heigtmapTexture.height];


        //for (int i = 0; i < heigtmapTexture.width; i++)
        //{
        //    for (int j = 0; j < heigtmapTexture.height; j++)
        //    {
        //        height[i, j] = heigtmapTexture.GetPixel(i, j);

        //    }
        //}

        //for (int i = 0; i < heigtmapTexture.width; i++)
        //{
        //    for (int j = 0; j < heigtmapTexture.height; j++)
        //    {
        //        newHeight[j, i] = height[i,j];
        //        heigtmapTexture.SetPixel(j, i, newHeight[j, i]);

        //    }
        //}
        newTexture.wrapMode = TextureWrapMode.Clamp;
        newTexture.filterMode = FilterMode.Point;
        newTexture.Apply();

        CreatePlane();

        this.GetComponent<Renderer>().material.SetTexture("_MainTex", newTexture);
        this.GetComponent<Renderer>().material.SetTexture("_DispTex", newTexture);
    }


    private void Update()
    {
        renderer.material.mainTexture = pixelTexture();
        heigtmapTexture = (Texture2D)renderer.material.mainTexture;

        Texture2D newTexture = new Texture2D(heigtmapTexture.width, heigtmapTexture.height);
        Graphics.CopyTexture(heigtmapTexture, newTexture);

        Color[] pixels = newTexture.GetPixels();
        Array.Reverse(pixels);
        newTexture.SetPixels(pixels);
        
        newTexture.wrapMode = TextureWrapMode.Clamp;
        newTexture.filterMode = FilterMode.Point;
        newTexture.Apply();

        this.GetComponent<Renderer>().material.SetTexture("_MainTex", newTexture);
        this.GetComponent<Renderer>().material.SetTexture("_DispTex", newTexture);
    }

    public Texture2D pixelTexture()
    {
        Texture2D generatedTexture = new Texture2D(widthSegments, lengthSegments);
        for (int i = 0; i < widthSegments; i++)
        {
            for (int j = 0; j < lengthSegments; j++)
            {

                Color color;

                int rand = UnityEngine.Random.Range(0, 2);
                if (rand == 0)
                {
                    color = Color.black;
                }
                else
                {
                    color = Color.white;
                }

                //if ((i > 20 && j > 40))
                //{
                //    color = Color.white;
                //}
                //else
                //{
                //    color = Color.black;
                //}
                generatedTexture.SetPixel(i, j, color);
            }
        }

        generatedTexture.wrapMode = TextureWrapMode.Clamp;
        generatedTexture.filterMode = FilterMode.Point;
        generatedTexture.Apply();

        return generatedTexture;
    }


    void CreatePlane()
    {


        this.transform.position = Vector3.zero;

        Vector2 anchorOffset;
        string anchorId;
        switch (anchor)
        {
            case AnchorPoint.TopLeft:
                anchorOffset = new Vector2(-width / 2.0f, length / 2.0f);
                anchorId = "TL";
                break;
            case AnchorPoint.TopHalf:
                anchorOffset = new Vector2(0.0f, length / 2.0f);
                anchorId = "TH";
                break;
            case AnchorPoint.TopRight:
                anchorOffset = new Vector2(width / 2.0f, length / 2.0f);
                anchorId = "TR";
                break;
            case AnchorPoint.RightHalf:
                anchorOffset = new Vector2(width / 2.0f, 0.0f);
                anchorId = "RH";
                break;
            case AnchorPoint.BottomRight:
                anchorOffset = new Vector2(width / 2.0f, -length / 2.0f);
                anchorId = "BR";
                break;
            case AnchorPoint.BottomHalf:
                anchorOffset = new Vector2(0.0f, -length / 2.0f);
                anchorId = "BH";
                break;
            case AnchorPoint.BottomLeft:
                anchorOffset = new Vector2(-width / 2.0f, -length / 2.0f);
                anchorId = "BL";
                break;
            case AnchorPoint.LeftHalf:
                anchorOffset = new Vector2(-width / 2.0f, 0.0f);
                anchorId = "LH";
                break;
            case AnchorPoint.Center:
            default:
                anchorOffset = Vector2.zero;
                anchorId = "C";
                break;
        }

        MeshFilter meshFilter = (MeshFilter)this.gameObject.GetComponent(typeof(MeshFilter));
        this.gameObject.GetComponent(typeof(MeshRenderer));

        string planeAssetName = this.name + widthSegments + "x" + lengthSegments + "W" + width + "L" + length + (orientation == Orientation.Horizontal ? "H" : "V") + anchorId + ".asset";
        Mesh m = null;

   
        m = new Mesh();
        m.name = this.gameObject.name;

        int hCount2 = widthSegments + 1;
        int vCount2 = lengthSegments + 1;
        int numTriangles = widthSegments * lengthSegments * 6;
        if (twoSided)
        {
            numTriangles *= 2;
        }
        int numVertices = hCount2 * vCount2;

        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uvs = new Vector2[numVertices];
        int[] triangles = new int[numTriangles];
        Vector4[] tangents = new Vector4[numVertices];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        int index = 0;
        float uvFactorX = 1.0f / widthSegments;
        float uvFactorY = 1.0f / lengthSegments;
        float scaleX = width / widthSegments;
        float scaleY = length / lengthSegments;
        for (float y = 0.0f; y < vCount2; y++)
        {
            for (float x = 0.0f; x < hCount2; x++)
            {
                if (orientation == Orientation.Horizontal)
                {
                    vertices[index] = new Vector3(x * scaleX - width / 2f - anchorOffset.x, 0.0f, y * scaleY - length / 2f - anchorOffset.y);
                }
                else
                {
                    vertices[index] = new Vector3(x * scaleX - width / 2f - anchorOffset.x, y * scaleY - length / 2f - anchorOffset.y, 0.0f);
                }
                tangents[index] = tangent;
                uvs[index++] = new Vector2(x * uvFactorX, y * uvFactorY);
            }
        }

        index = 0;
        for (int y = 0; y < lengthSegments; y++)
        {
            for (int x = 0; x < widthSegments; x++)
            {
                triangles[index] = (y * hCount2) + x;
                triangles[index + 1] = ((y + 1) * hCount2) + x;
                triangles[index + 2] = (y * hCount2) + x + 1;

                triangles[index + 3] = ((y + 1) * hCount2) + x;
                triangles[index + 4] = ((y + 1) * hCount2) + x + 1;
                triangles[index + 5] = (y * hCount2) + x + 1;
                index += 6;
            }
            if (twoSided)
            {
                // Same tri vertices with order reversed, so normals point in the opposite direction
                for (int x = 0; x < widthSegments; x++)
                {
                    triangles[index] = (y * hCount2) + x;
                    triangles[index + 1] = (y * hCount2) + x + 1;
                    triangles[index + 2] = ((y + 1) * hCount2) + x;

                    triangles[index + 3] = ((y + 1) * hCount2) + x;
                    triangles[index + 4] = (y * hCount2) + x + 1;
                    triangles[index + 5] = ((y + 1) * hCount2) + x + 1;
                    index += 6;
                }
            }
        }

        m.vertices = vertices;
        m.uv = uvs;
        m.triangles = triangles;
        m.tangents = tangents;
        m.RecalculateNormals();

        

        meshFilter.sharedMesh = m;
        m.RecalculateBounds();

        if (addCollider)
            this.gameObject.AddComponent(typeof(BoxCollider));

    }
}
