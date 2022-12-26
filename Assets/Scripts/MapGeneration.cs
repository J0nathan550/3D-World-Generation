using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;

public class MapGeneration : MonoBehaviour
{
    public UISystem ui;
    public float minZ, maxZ; // height
    public float freqX = 1f;
    public float freqY = 1f;
    public int seed;
    public Color color0;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
    public Color color5;
    public string mapName = "";

    public Vector3 scale = Vector3.one;


    [Range(2, 8)]
    public int size = 4;
    public List<Transform> maps = new();
    [SerializeField] private GameObject originalMap;
    public GameObject map; 
    [SerializeField] private Texture2D defaultMap;
    [SerializeField] private Texture2D selectedMap;
    private MeshRenderer selectedMesh = null;
    private Material selectedMaterial = null;   
    [SerializeField] private GameObject selectionObject;
    [SerializeField] private Material wireFrameMat, terrainMat;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material blankMaterial;

    private void Start()
    {
        map = Instantiate(originalMap, transform);
        map.GetComponent<MeshRenderer>().material = blankMaterial;
        map.transform.position = Vector3.zero;
        maps.Add(map.transform);

        map.name = "first";
        

    }

    private void Update()
    {
        if (MoveCamera.fullscreen)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && !ui.arePanelsOpen)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit info))
            {
                if (info.collider.TryGetComponent(out MeshRenderer MR))
                {
                    if (info.collider.CompareTag("Selection"))
                    {
                        foreach (Transform t in maps)
                        {
                            if (t.position == info.collider.transform.position)
                            {
                                return;
                            }
                        }
                        GameObject map = Instantiate(originalMap, transform);
                        map.GetComponent<MeshRenderer>().material = blankMaterial;
                        map.transform.position = info.collider.transform.position;
                        maps.Add(map.transform);
                        map.name = $"MAP: {transform.childCount}";
                        if (selectedMesh != null)
                        {
                            //selectedMesh.material.SetTexture("_Tex", defaultMap);
                            selectedMesh.material = selectedMaterial;
                        }
                        selectedMesh = null;
                        selectedMaterial = null;
                        selectionObject.SetActive(false);
                    }
                    else
                    {
                        if (selectedMesh != null)
                        {
                            selectedMesh.material = selectedMaterial;
                        }
                        //MR.material.SetTexture("_Tex", selectedMap);
                        selectedMaterial = MR.material;
                        MR.material = yellowMaterial;
                        selectedMesh = MR;
                        selectionObject.SetActive(true);
                        selectionObject.transform.position = MR.transform.position;
                    }
                }
            }
            else
            {
                if (selectedMesh != null)
                {
                    selectedMesh.material = selectedMaterial;
                }
                selectionObject.SetActive(false);
                selectedMesh = null;    
                selectedMaterial = null;
            }
        }
    } 

    private float[,] values;

    private int sizeX = 256, sizeY = 256; // quality

    public void WireFrameMode(bool isWireFrameMode)
    {
        foreach (Transform item in maps)
        {
            item.TryGetComponent(out MeshRenderer mr);
            if (isWireFrameMode)
            {
                mr.material = wireFrameMat;
            }
            else
            {
                mr.material = terrainMat;
            }
        }
    }

    public Texture2D GenerateTexture(Transform map)
    {
        float offX = map.position.x + rndOffX;
        float offY = map.position.z + rndOffY;

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

        Texture2D txt = new Texture2D(sizeX, sizeY, TextureFormat.ARGB32, false, false);
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                float pX = (offX + (x * 1f / sizeX)) * freqX;
                float pY = (offY + (y * 1f / sizeY)) * freqY;
                float h = Mathf.PerlinNoise(pX, pY);
                h = minZ + h * (maxZ - minZ);

                Color color;
                if (h < .2f)
                {
                    color = Color.Lerp(color0, color1, h * 0.2f);
                }
                else if (h < .4f)
                {
                    color = Color.Lerp(color1, color2, (h - .2f) * 0.2f);
                }
                else if (h < .6f)
                {
                    color = Color.Lerp(color2, color3, (h - .4f) * 0.2f);
                }
                else if (h < .8f)
                {
                    color = Color.Lerp(color3, color4, (h - .6f) * 0.2f);
                }
                else
                {
                    color = Color.Lerp(color4, color5, (h - .8f) * 0.2f);
                }

                txt.SetPixel(x, y, color);
                values[x,y] = h;
            }
        }
        txt.Apply();
        return txt; 
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

        foreach (Transform item in maps)
        {
            Destroy(item.gameObject);
        }
        GameObject map = Instantiate(originalMap, transform);
        map.GetComponent<MeshRenderer>().material = blankMaterial;
        map.transform.position = Vector3.zero;
        maps.Clear();
        maps.Add(map.transform);
    }

    private float rndOffX, rndOffY;

    public void GenerateMesh()
    {
        Rnd rnd = new Rnd(seed); // Never using
        rndOffX = rnd.Get(-10f, 10f);
        rndOffY = rnd.Get(-10f, 10f);
        foreach (Transform map in maps)
        {
            Texture2D txt = GenerateTexture(map);

            transform.localScale = Vector3.one;

            Mesh mesh = new Mesh();
            var vertices = new Vector3[(sizeX * sizeY)];
            for (int i = 0, y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++, i++)
                {
                    vertices[i] = new Vector3(x * 1f / sizeX - .5f, values[x, y], y * 1f / sizeY - .5f); // hello?
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

            map.TryGetComponent(out MeshRenderer mR);
            map.TryGetComponent(out MeshFilter mF);

            mF.mesh = mesh;
            mesh.name = "Procedural Grid";

            if (UISystem.isWireFrameMode)
            {
                mR.material = wireFrameMat;
            }
            else
            {
                mR.material = terrainMat;
                Material material = mR.material;
                mR.material = material;
                mR.material.SetTexture("_Tex", txt);
            }
        }
        transform.localScale = scale;
        ui.UpdateUI();
        ui.Saving();
    }

    public void UpdateMaps(List<MapPosition> positionList)
    {
        foreach (Transform item in maps)
        {
            Destroy(item.gameObject);
        }
        maps.Clear();
        foreach (var mapPos in positionList)
        {
            GameObject map = Instantiate(originalMap, transform);
            map.GetComponent<MeshRenderer>().material = blankMaterial;
            map.transform.localPosition = mapPos.v3();
            map.name = $"MAP: {transform.childCount}";
            maps.Add(map.transform);
        }
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