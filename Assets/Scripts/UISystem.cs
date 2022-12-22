using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using Ookii.Dialogs;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UISystem : MonoBehaviour
{
    [Header("Panels")]
    public GameObject[] panels;
    [Header("Color Panels")]
    public Button[] colorButtons;
    public GameObject[] colorPanels;
    [SerializeField] private Slider[] color0RGB, color1RGB, color2RGB, color3RGB, color4RGB, color5RGB;
    [SerializeField] private TextMeshProUGUI[] color0RGBText, color1RGBText, color2RGBText, color3RGBText, color4RGBText, color5RGBText;
    [SerializeField] private Image[] colorImages;
    [Header("Other elements")]

    public GameObject[] allUIElements;
    public MapGeneration generation;
    public MoveCamera moveCamera;

    [SerializeField] private TMP_InputField cameraSpeedText, mouseText;
    [SerializeField] private TMP_InputField mapNameText,sizeXText, sizeYText, sizeZText, seedText;
    [SerializeField] private TextMeshProUGUI sizeXTextUI, sizeYTextUI, sizeZTextUI;
    [SerializeField] private TextMeshProUGUI mapNameLabel, seedLabel;
    [SerializeField] private TextMeshProUGUI sizeHMin, sizeHMax, sizeHMinValue, sizeHMaxValue;
    [SerializeField] private Slider sizeHMinSlider, sizeHMaxSlider;
    [SerializeField] private Transform world;

    private int lastPanel;
    private int lastColorPanel;

    [SerializeField] private Material terrain, wireframe;

    [SerializeField] private Toggle wireFrameToggle;
    [SerializeField] private Transform playerPos;

    public MapSavingSystem savingSystem;

    private void Start()
    {
        savingSystem = new MapSavingSystem();
        moveCamera.cameraSpeed = PlayerPrefs.GetFloat("cameraSpeed", 4f);
        moveCamera.mouseSensitivity = PlayerPrefs.GetFloat("mouseSensitivity", 4f);
        cameraSpeedText.text = moveCamera.cameraSpeed.ToString();
        mouseText.text = moveCamera.mouseSensitivity.ToString();
    }
    public static bool isOpenPanel = false;
    public void OpenClosePanel(int index) 
    {
        isOpenPanel = !isOpenPanel;
        lastPanel = index;
        bool active = panels[index].activeSelf;
        panels[index].SetActive(!active);
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == lastPanel)
            {
                continue;
            }
            panels[i].SetActive(false);
        }
    }

    public void ChangeSquareColor(int index)
    {
        Color32[] color = new Color32[6];
        switch (index)
        {
            case 0:
                color0RGBText[0].text = color0RGB[0].value.ToString();
                color0RGBText[1].text = color0RGB[1].value.ToString();
                color0RGBText[2].text = color0RGB[2].value.ToString();
                color[0].r = (byte)color0RGB[0].value;
                color[0].g = (byte)color0RGB[1].value;
                color[0].b = (byte)color0RGB[2].value;
                color[0].a = 255;
                colorImages[0].color = color[0];
                generation.color0 = color[0];
                var c = colorButtons[0].colors;
                c.normalColor = color[0];
                colorButtons[0].colors = c;
                savingSystem.colors[0] = new MapColorTest(color0RGB[0].value, color0RGB[1].value, color0RGB[2].value);
                break;
            case 1:
                color1RGBText[0].text = color1RGB[0].value.ToString();
                color1RGBText[1].text = color1RGB[1].value.ToString();
                color1RGBText[2].text = color1RGB[2].value.ToString();
                color[1].r = (byte)color1RGB[0].value;
                color[1].g = (byte)color1RGB[1].value;
                color[1].b = (byte)color1RGB[2].value;
                color[1].a = 255;
                colorImages[1].color = color[1];
                generation.color1 = color[1];
                var c1 = colorButtons[1].colors;
                c1.normalColor = color[1];
                colorButtons[1].colors = c1;
                savingSystem.colors[1] = new MapColorTest(color1RGB[0].value, color1RGB[1].value, color1RGB[2].value);
                break;
            case 2:
                color2RGBText[0].text = color2RGB[0].value.ToString();
                color2RGBText[1].text = color2RGB[1].value.ToString();
                color2RGBText[2].text = color2RGB[2].value.ToString();
                color[2].r = (byte)color2RGB[0].value;
                color[2].g = (byte)color2RGB[1].value;
                color[2].b = (byte)color2RGB[2].value;
                color[2].a = 255;
                colorImages[2].color = color[2];
                generation.color2 = color[2];
                var c2 = colorButtons[2].colors;
                c2.normalColor = color[2];
                colorButtons[2].colors = c2;
                savingSystem.colors[2] = new MapColorTest(color2RGB[0].value, color2RGB[1].value, color2RGB[2].value);
                break;
            case 3:
                color3RGBText[0].text = color3RGB[0].value.ToString();
                color3RGBText[1].text = color3RGB[1].value.ToString();
                color3RGBText[2].text = color3RGB[2].value.ToString();
                color[3].r = (byte)color3RGB[0].value;
                color[3].g = (byte)color3RGB[1].value;
                color[3].b = (byte)color3RGB[2].value;
                color[3].a = 255;
                colorImages[3].color = color[3];
                generation.color3 = color[3];
                var c3 = colorButtons[3].colors;
                c3.normalColor = color[3];
                colorButtons[3].colors = c3;
                savingSystem.colors[3] = new MapColorTest(color3RGB[0].value, color3RGB[1].value, color3RGB[2].value);
                break;
            case 4:
                color4RGBText[0].text = color4RGB[0].value.ToString();
                color4RGBText[1].text = color4RGB[1].value.ToString();
                color4RGBText[2].text = color4RGB[2].value.ToString();
                color[4].r = (byte)color4RGB[0].value;
                color[4].g = (byte)color4RGB[1].value;
                color[4].b = (byte)color4RGB[2].value;
                color[4].a = 255;
                colorImages[4].color = color[4];
                generation.color4 = color[4];
                var c4 = colorButtons[4].colors;
                c4.normalColor = color[4];
                colorButtons[4].colors = c4;
                savingSystem.colors[4] = new MapColorTest(color4RGB[0].value, color4RGB[1].value, color4RGB[2].value);
                break;
            case 5:
                color5RGBText[0].text = color5RGB[0].value.ToString();
                color5RGBText[1].text = color5RGB[1].value.ToString();
                color5RGBText[2].text = color5RGB[2].value.ToString();
                color[5].r = (byte)color5RGB[0].value;
                color[5].g = (byte)color5RGB[1].value;
                color[5].b = (byte)color5RGB[2].value;
                color[5].a = 255;
                colorImages[5].color = color[5];
                generation.color5 = color[5];
                var c5 = colorButtons[5].colors;
                c5.normalColor = color[5];
                colorButtons[5].colors = c5;
                savingSystem.colors[5] = new MapColorTest(color5RGB[0].value, color5RGB[1].value, color5RGB[2].value);
                break;
        }
    }

    public void Export()
    {
        savingSystem.positionList.Clear();
        foreach (Transform t in generation.transform)
        {
            savingSystem.positionList.Add(new()
            {
                x = t.localPosition.x,
                y = t.localPosition.y,
                z = t.localPosition.z,
            });
        }
        savingSystem.cameraPos = new()
        {
            x = playerPos.position.x,
            y = playerPos.position.y,
            z = playerPos.position.z,
            rotX = playerPos.eulerAngles.x,
            rotY = playerPos.eulerAngles.y,
            rotZ = playerPos.eulerAngles.z,
        };
        string json = JsonConvert.SerializeObject(savingSystem, Formatting.Indented);
        VistaSaveFileDialog createFile = new VistaSaveFileDialog();
        if (createFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            File.WriteAllText(createFile.FileName, json);
        }
    }

    public void Import()
    {
        try
        {
            VistaOpenFileDialog openFile = new VistaOpenFileDialog();
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string json = File.ReadAllText(openFile.FileName);
                savingSystem = JsonConvert.DeserializeObject<MapSavingSystem>(json);
            }
            LoadingSettings();
            Destroy(generation.map.gameObject);
            generation.maps.Remove(generation.map.transform);
            generation.UpdateMaps(savingSystem.positionList);
            generation.GenerateMesh();
        }
        catch (System.Exception)
        {

        }
    }

    private void LoadingSettings()
    {
        generation.seed = savingSystem.seed;
        generation.mapName = savingSystem.mapName;
        generation.maxZ = savingSystem.maxH;
        generation.minZ = savingSystem.minH;

        playerPos.position = new Vector3(savingSystem.cameraPos.x, savingSystem.cameraPos.y, savingSystem.cameraPos.z);
        playerPos.eulerAngles = new Vector3(savingSystem.cameraPos.rotX, savingSystem.cameraPos.rotY, savingSystem.cameraPos.rotZ);

        world.transform.localScale = new Vector3(savingSystem.sizeX, savingSystem.sizeY, savingSystem.sizeZ);

        seedText.text = savingSystem.seed.ToString();
        seedLabel.text = "Seed: {savingSystem.seed}";
        mapNameText.text = savingSystem.mapName.ToString();
        mapNameLabel.text = $"Name: {savingSystem.mapName}";
        sizeXText.text = savingSystem.sizeX.ToString();
        sizeXTextUI.text = $"Size X: {savingSystem.sizeX}";
        sizeYText.text = savingSystem.sizeY.ToString();
        sizeYTextUI.text = $"Size Y: {savingSystem.sizeY}";
        sizeZText.text = savingSystem.sizeZ.ToString();
        sizeZTextUI.text = $"Size Z: {savingSystem.sizeZ}";

        sizeHMin.text = $"Min Size H: {savingSystem.minH.ToString("F2")}";
        sizeHMax.text = $"Max Size H: {savingSystem.maxH.ToString("F2")}";
        sizeHMaxSlider.value = savingSystem.maxH;   
        sizeHMinSlider.value = savingSystem.minH;
        sizeHMaxValue.text = savingSystem.maxH.ToString("F2");
        sizeHMinValue.text = savingSystem.maxH.ToString("F2");

        color0RGBText[0].text = savingSystem.colors[0].r.ToString();
        color0RGBText[1].text = savingSystem.colors[0].g.ToString();
        color0RGBText[2].text = savingSystem.colors[0].b.ToString();

        color1RGBText[0].text = savingSystem.colors[1].r.ToString();
        color1RGBText[1].text = savingSystem.colors[1].g.ToString();
        color1RGBText[2].text = savingSystem.colors[1].b.ToString();

        color2RGBText[0].text = savingSystem.colors[2].r.ToString();
        color2RGBText[1].text = savingSystem.colors[2].g.ToString();
        color2RGBText[2].text = savingSystem.colors[2].b.ToString();

        color3RGBText[0].text = savingSystem.colors[3].r.ToString();
        color3RGBText[1].text = savingSystem.colors[3].g.ToString();
        color3RGBText[2].text = savingSystem.colors[3].b.ToString();

        color4RGBText[0].text = savingSystem.colors[4].r.ToString();
        color4RGBText[1].text = savingSystem.colors[4].g.ToString();
        color4RGBText[2].text = savingSystem.colors[4].b.ToString();

        color5RGBText[0].text = savingSystem.colors[5].r.ToString();
        color5RGBText[1].text = savingSystem.colors[5].g.ToString();
        color5RGBText[2].text = savingSystem.colors[5].b.ToString();

        color0RGB[0].SetValueWithoutNotify(savingSystem.colors[0].r); 
        color0RGB[1].SetValueWithoutNotify(savingSystem.colors[0].g);  
        color0RGB[2].SetValueWithoutNotify(savingSystem.colors[0].b);

        color1RGB[0].SetValueWithoutNotify(savingSystem.colors[1].r);
        color1RGB[1].SetValueWithoutNotify(savingSystem.colors[1].g);
        color1RGB[2].SetValueWithoutNotify(savingSystem.colors[1].b);

        color2RGB[0].SetValueWithoutNotify(savingSystem.colors[2].r);
        color2RGB[1].SetValueWithoutNotify(savingSystem.colors[2].g);
        color2RGB[2].SetValueWithoutNotify(savingSystem.colors[2].b);

        color3RGB[0].SetValueWithoutNotify(savingSystem.colors[3].r);
        color3RGB[1].SetValueWithoutNotify(savingSystem.colors[3].g);
        color3RGB[2].SetValueWithoutNotify(savingSystem.colors[3].b);

        color4RGB[0].SetValueWithoutNotify(savingSystem.colors[4].r);
        color4RGB[1].SetValueWithoutNotify(savingSystem.colors[4].g);
        color4RGB[2].SetValueWithoutNotify(savingSystem.colors[4].b);

        color5RGB[0].SetValueWithoutNotify(savingSystem.colors[5].r);
        color5RGB[1].SetValueWithoutNotify(savingSystem.colors[5].g);
        color5RGB[2].SetValueWithoutNotify(savingSystem.colors[5].b);

        for (int i = 0; i < colorButtons.Length; i++)
        {
            Color32 colorButton = new Color32((byte)savingSystem.colors[i].r, (byte)savingSystem.colors[i].g, (byte)savingSystem.colors[i].b, 255);
            var c = colorButtons[i].colors;
            c.normalColor = colorButton;
            colorButtons[i].colors = c;
        }

        for (int i = 0; i < colorImages.Length; i++)
        {
            Color32 colorParam = new Color32((byte)savingSystem.colors[i].r, (byte)savingSystem.colors[i].g, (byte)savingSystem.colors[i].b, 255);
            colorImages[i].color = colorParam;
            if (i == 0) generation.color0 = colorParam;
            else if (i == 1) generation.color1 = colorParam;
            else if (i == 2) generation.color2 = colorParam;
            else if (i == 3) generation.color3 = colorParam;
            else if (i == 4) generation.color4 = colorParam;
            else if (i == 5) generation.color5 = colorParam;
        }
        generation.GenerateMesh();
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void OpenCloseColorPanel(int index)
    {
        lastColorPanel = index;
        bool active = colorPanels[index].activeSelf;
        colorPanels[index].SetActive(!active);
        for (int i = 0; i < colorPanels.Length; i++)
        {
            if (i == lastColorPanel)
            {
                continue;
            }
            colorPanels[i].SetActive(false);
        }
    }

    public void NewMap()
    {
        generation.ClearAll();
        SceneManager.LoadScene(0);
    }

    public static bool isWireFrameMode;

    public void WireFrameMode()
    {
        isWireFrameMode = !isWireFrameMode;
        generation.WireFrameMode(isWireFrameMode);
    }

    public void SavePlayerSettings()
    {
        moveCamera.cameraSpeed = float.Parse(cameraSpeedText.text);
        moveCamera.mouseSensitivity = float.Parse(mouseText.text);
        PlayerPrefs.SetFloat("cameraSpeed", moveCamera.cameraSpeed);
        PlayerPrefs.SetFloat("mouseSensitivity", moveCamera.mouseSensitivity);
    }

    public void UpdateUI()
    {
        generation.mapName = mapNameText.text;
        world.transform.localScale = new Vector3(float.Parse(sizeXText.text), float.Parse(sizeYText.text), float.Parse(sizeZText.text));
        generation.minZ = sizeHMinSlider.value;
        generation.maxZ = sizeHMaxSlider.value;
        generation.seed = int.Parse(seedText.text);

        mapNameLabel.text = $"Name: {mapNameText.text}";
        seedLabel.text = $"Seed: {generation.seed}";
        sizeXTextUI.text = $"Size X: {sizeXText.text}";
        sizeYTextUI.text = $"Size Y: {sizeYText.text}";
        sizeZTextUI.text = $"Size Z: {sizeZText.text}";
        sizeHMinSlider.value = generation.minZ;
        sizeHMaxSlider.value = generation.maxZ;
        sizeHMinValue.text = $"{sizeHMinSlider.value.ToString("F2")}";
        sizeHMaxValue.text = $"{sizeHMinSlider.value.ToString("F2")}";
    }

    public void Saving()
    {
        savingSystem.mapName = mapNameText.text;
        savingSystem.sizeX = float.Parse(sizeXText.text);
        savingSystem.sizeY = float.Parse(sizeYText.text);
        savingSystem.sizeZ = float.Parse(sizeZText.text);
        savingSystem.seed = int.Parse(seedText.text);
        savingSystem.minH = generation.minZ;
        savingSystem.maxH = generation.maxZ;
    }

    public void ChangeSizeHSlider(int slider)
    {
        if (slider == 0)
        {
            sizeHMinValue.text = sizeHMinSlider.value.ToString("F2");
            sizeHMin.text = $"Min Size H: {sizeHMinSlider.value.ToString("F2")}";
        }
        if (slider == 1)
        {
            sizeHMaxValue.text = sizeHMaxSlider.value.ToString("F2");
            sizeHMax.text = $"Max Size H: {sizeHMaxSlider.value.ToString("F2")}";
        }
    }

    public void RandomSeed()
    {
        seedText.text = Random.Range(0, int.MaxValue).ToString();
        savingSystem.seed = int.Parse(seedText.text);
        seedLabel.text = $"Seed: {seedText.text}";
    }

}

[System.Serializable]
public class MapSavingSystem
{
    public string mapName { get; set; }
    public float sizeX { get; set; }
    public float sizeY { get; set; }
    public float sizeZ { get; set; }
    public float minH { get; set; }
    public float maxH { get; set; }
    public int seed { get; set; }
    public MapColorTest[] colors { get; set; }
    public List<MapPosition> positionList { get; set; }
    public CameraPosition cameraPos { get; set; }
    public MapSavingSystem()
    {
        colors = new MapColorTest[6]
        {
            new MapColorTest(255,255,255),
            new MapColorTest(255,255,255),
            new MapColorTest(255,255,255),
            new MapColorTest(255,255,255),
            new MapColorTest(255,255,255),
            new MapColorTest(255,255,255),
        };
        positionList = new List<MapPosition>();
        seed = 0;
        mapName = "Unknown";
        sizeX = 1;
        sizeY = 1;
        sizeZ = 1;
        cameraPos = new CameraPosition()
        {
            x = -0.7967257f,
            y = 0.387666f,   
            z = -0.8068401f,
            rotX = 21.037f,
            rotY = 46.325f,
            rotZ = 0f
        };
    }
}

[System.Serializable]
public class MapColorTest
{

    public MapColorTest(float r, float g, float b)
    {
        this.r = (int)r;
        this.g = (int)g;
        this.b = (int)b;
    }

    public int r { get; set; }
    public int g { get; set; }
    public int b { get; set; }
}

[System.Serializable]
public class MapPosition
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    internal Vector3 v3()
    {
        return new Vector3(x, y, z);
    }
}
[System.Serializable]
public class CameraPosition
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public float rotX { get; set; }
    public float rotY { get; set; }
    public float rotZ { get; set; }
}