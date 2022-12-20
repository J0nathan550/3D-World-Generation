using UnityEngine;
using UnityEditor;
using TMPro;

public class MapGeneration : MonoBehaviour
{
    private int sizeX = 256, sizeY = 256;
    public float minZ, maxZ;

    [SerializeField] MeshRenderer mR;
    [SerializeField] MeshFilter mF;
    public int seed;

    private Texture2D txt;
    private float[,] values;

    public Color color0;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
    public Color color5;

    [SerializeField] private TextMeshProUGUI sizeMinZText, sizeMaxZText, seedText, mapNameText;

    public string mapName = "";

    public void GenerateTexture()
    {
        Rnd rnd = new Rnd(seed);

        float offX = rnd.Get(), offY = rnd.Get();
        float freqX = rnd.Get(1,5f), freqY = rnd.Get(1,5f);

        if (minZ < 0)
        {
            minZ = 0;
        }
        if (maxZ <= minZ)
        {
            maxZ = 1;
        }
        if (maxZ > 1)
        {
            maxZ = 1;
        }

        values = new float[sizeX, sizeY];

        txt = new Texture2D(sizeX, sizeY, TextureFormat.ARGB32, false, false);
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                float pX = offX + (x * 1f / sizeX) * freqX;
                float pY = offY + (y * 1f / sizeY) * freqY;
                float h = Mathf.PerlinNoise(pX, pY);
                h = minZ + h * (maxZ - minZ);

                Color color;
                if (h < .2f)
                {
                    color = Color.Lerp(color0, color1, h * 1.5f);
                }
                else if (h < .4f)
                {
                    color = Color.Lerp(color1, color2, (h - .2f) * 1.5f);
                }
                else if (h < .6f)
                {
                    color = Color.Lerp(color2, color3, (h - .4f) * 1.5f);
                }
                else if (h < .8f)
                {
                    color = Color.Lerp(color3, color4, (h - .6f) * 1.5f);
                }
                else
                {
                    color = Color.Lerp(color4, color5, (h - .8f) * 1.5f);
                }

                txt.SetPixel(x, y, color);
                values[x,y] = h;
            }
        }
        txt.Apply();
        if (!UISystem.isWireFrameMode)
        {
            mR.sharedMaterial.SetTexture("_Tex", txt);
        }

    }

    private void Update()
    {
        sizeMinZText.text = $"Size Min Z: {minZ}";
        sizeMaxZText.text = $"Size Max Z: {maxZ}";
        seedText.text = $"Seed: {seed}";
        mapNameText.text = $"Name: {mapName}";
    }

    public void ClearAll()
    {
        mapName = "Unknown";
        minZ = 0;
        maxZ = 1;
        seed = 0;

        Texture2D txt = new Texture2D(sizeX, sizeY, TextureFormat.ARGB32, false);
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                txt.SetPixel(x, y, Color.white);
            }
        }
        txt.Apply();
        mR.sharedMaterial.SetTexture("_Tex", txt);
        Mesh mesh = new Mesh();
        Vector3[] points = new Vector3[4];
        points[0] = new Vector3(0, 0, 0);
        points[1] = new Vector3(1, 0, 0);
        points[2] = new Vector3(1, 0, 1);
        points[3] = new Vector3(0, 0, 1);
        mesh.vertices = points;
        int[] triangles = new int[6];
        triangles[0] = 0;   
        triangles[1] = 3;   
        triangles[2] = 2;   
        triangles[3] = 0;   
        triangles[4] = 2;   
        triangles[5] = 1;
        mesh.triangles = triangles;
        Vector2[] UV = new Vector2[4];
        UV[0] = new Vector2(0, 0);  
        UV[1] = new Vector2(1, 0);
        UV[2] = new Vector2(1, 1);
        UV[3] = new Vector2(0, 1);
        mesh.uv = UV;
        mesh.RecalculateNormals();
        mF.mesh = mesh;
    }

    public void GenerateMesh()
    {
        GenerateTexture();
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Grid";

        var vertices = new Vector3[(sizeX * sizeY)];
        for (int i = 0, y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++, i++)
            {
                vertices[i] = new Vector3(x * 1f / sizeX, values[x,y], y * 1f / sizeY);
            }
        }
        mesh.vertices = vertices;

        int[] triangles = new int[sizeX * sizeY * 6];
        int index = 0;
        for (int y = 0; y < sizeY - 1; y++)
        {
            for (int x = 0; x < sizeX - 1; x++)
            {
                triangles[index + 0] = x + (sizeX) * y;
                triangles[index + 1] = x + 1 + (sizeX) * (y + 1);
                triangles[index + 2] = x + 1 + (sizeX) * y;
                triangles[index + 3] = x + (sizeX) * y;
                triangles[index + 4] = x + (sizeX) * (y + 1);
                triangles[index + 5] = x + 1 + (sizeX) * (y + 1);
                index += 6;
            }
        }

        mesh.triangles = triangles;

        Vector2[] UV = new Vector2[sizeX * sizeY];

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                UV[x + sizeX * y] = new Vector2(x * 1f / sizeX, y * 1f / sizeY);
            }
        }

        mesh.uv = UV;

        mesh.RecalculateNormals();
        mF.mesh = mesh;
    }
}
#if UNITY_EDITOR

[CustomEditor(typeof(MapGeneration))]
public class MapGenerationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Texture"))
        {
            var tgt = (MapGeneration)target;
            tgt.GenerateTexture();
        }
        if (GUILayout.Button("Clear all"))
        {
            var tgt = (MapGeneration)target;
            tgt.ClearAll();
        }
        if (GUILayout.Button("Generate Mesh"))
        {
            var tgt = (MapGeneration)target;
            tgt.GenerateMesh();
        }
        if (GUILayout.Button("Random Seed"))
        {
            var tgt = (MapGeneration)target;
            tgt.seed = Random.Range(0,int.MaxValue);
            tgt.GenerateMesh();
        }
        GUILayout.EndHorizontal();
    }

}
#endif 