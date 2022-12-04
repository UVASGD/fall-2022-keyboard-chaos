using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ElectorchStrauss.ProceduralTerrainSystem.Scripts
{
    public class TerrainRuntimeModification : MonoBehaviour
    {
        public MapGenerator terrainGenerator;
        public NoiseData noiseData;
        public TerrainData terrainData;
        public ColorData colorData;
        public string path;
        public Slider seedSlider,
            scaleSlider,
            lacunaritySlider,
            persistanceSlider,
            octavesSlider,
            heightMeshMultiplierSlider;

        public Toggle flatShadingToggle, useFallOffToggle, useFallOnToggle;
        public RectTransform regionsPanel;
        public Button randomizeNoise;

        private void Start()
        {
            if (noiseData != null)
            {
                seedSlider.value = noiseData.seed;
                scaleSlider.value = noiseData.scale;
                lacunaritySlider.value = noiseData.lacunarity;
                persistanceSlider.value = noiseData.persistance;
                octavesSlider.value = noiseData.octaves;
            }

            if (terrainData != null)
            {
                heightMeshMultiplierSlider.value = terrainData.meshHeightMultiplier;
                useFallOffToggle.isOn = terrainData.useFalloff;
                useFallOnToggle.isOn = terrainData.useFallon;
                flatShadingToggle.isOn = terrainData.useFlatShading;
            }

            CreateRegions();
        }

        public void ImportMeshFbx()
        {
#if UNITY_EDITOR
            path = EditorUtility.OpenFilePanel("Import terrain mesh", "/ProceduralTerrainSystem/GeneratedTerrainMesh/",
                "fbx");
#endif
        }

        public void RandomizeColor()
        {
            for (int i = 0; i < colorData.regions.Length; i++)
            {
                colorData.regions[i].colour = Random.ColorHSV();
            }

            terrainGenerator.GenerateMap();
        }

        public void RandomizeNoise()
        {
            seedSlider.value = Random.Range(-100000, 100000);
            scaleSlider.value = Random.Range(1, 501);
            lacunaritySlider.value = Random.Range(1f, 2.6f);
            persistanceSlider.value = Random.Range(0f, 2f);
            octavesSlider.value = Random.Range(1, 7);
        }

        public void RandomizeTerrain()
        {
            heightMeshMultiplierSlider.value = Random.Range(0, 350);
            useFallOffToggle.isOn = (Random.value > 0.5f);
            useFallOnToggle.isOn = (Random.value > 0.5f);
            flatShadingToggle.isOn = (Random.value > 0.5f);
        }

        void RemoveRegions()
        {
            if (regionsPanel.childCount > 0)
            {
                for (int i = 0; i < regionsPanel.childCount; i++)
                {
                    Destroy(regionsPanel.GetChild(i).gameObject);
                }
            }
        }

        public void RegionsRemoveButton()
        {
            RemoveRegions();
            if (colorData.regions.Length > 0)
            {
                //remember the regions
                int colorDataRegionCount = colorData.regions.Length;
                string[] colorDataName = new string[colorDataRegionCount];
                float[] colorDataHeight = new float[colorDataRegionCount];
                Color[] colorDataColour = new Color[colorDataRegionCount];
                for (int i = 0; i < colorDataRegionCount; i++)
                {
                    colorDataName[i] = colorData.regions[i].name;
                    colorDataHeight[i] = colorData.regions[i].height;
                    colorDataColour[i] = colorData.regions[i].colour;
                }

                colorData.regions = new ColorData.TerrainType[colorDataRegionCount - 1];
                for (int i = 0; i < colorDataRegionCount - 1; i++)
                {
                    colorData.regions[i].name = colorDataName[i];
                    colorData.regions[i].height = colorDataHeight[i];
                    colorData.regions[i].colour = colorDataColour[i];
                }
            }

            CreateRegions();
        }

        public void RegionsAddButton()
        {
            RemoveRegions();
            if (colorData.regions.Length > 0)
            {
                //remember the regions
                int colorDataRegionCount = colorData.regions.Length;
                string[] colorDataName = new string[colorDataRegionCount];
                float[] colorDataHeight = new float[colorDataRegionCount];
                Color[] colorDataColour = new Color[colorDataRegionCount];
                for (int i = 0; i < colorDataRegionCount; i++)
                {
                    colorDataName[i] = colorData.regions[i].name;
                    colorDataHeight[i] = colorData.regions[i].height;
                    colorDataColour[i] = colorData.regions[i].colour;
                }

                colorData.regions = new ColorData.TerrainType[colorDataRegionCount + 1];
                for (int i = 0; i < colorDataRegionCount; i++)
                {
                    colorData.regions[i].name = colorDataName[i];
                    colorData.regions[i].height = colorDataHeight[i];
                    colorData.regions[i].colour = colorDataColour[i];
                }
            }

            CreateRegions();
        }

        void CreateRegions()
        {
            for (int i = 0; i < colorData.regions.Length; i++)
            {
                GameObject panel = new GameObject("Panel");
                panel.AddComponent<CanvasRenderer>();
                Image imagePanel = panel.AddComponent<Image>();
                Color tempColor = imagePanel.color;
                tempColor.a = 0.2f;
                imagePanel.color = tempColor;
                panel.transform.SetParent(regionsPanel.transform, false);
                RectTransform panelRect = panel.transform.GetComponent<RectTransform>();
                panelRect.anchorMin = new Vector2(0.5f, 1);
                panelRect.anchorMax = new Vector2(0.5f, 1);
                panelRect.pivot = new Vector2(0.5f, 1);
                panelRect.anchoredPosition = new Vector2(0, 0 - (i * (regionsPanel.sizeDelta.y / colorData.regions.Length)));
                panelRect.sizeDelta = new Vector2(regionsPanel.sizeDelta.x, regionsPanel.sizeDelta.y / colorData.regions.Length);

                //first one is the name of the regions so a input field again
                GameObject regionName = new GameObject("RegionNameInputField" + i);
                regionName.AddComponent<CanvasRenderer>();
                Image regionImage = regionName.AddComponent<Image>();
                regionImage.type = Image.Type.Sliced;
                regionImage.fillCenter = true;
#if UNITY_EDITOR
                regionImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/InputFieldBackground.psd");
#endif
                TMP_InputField regionInputField = regionName.AddComponent<TMP_InputField>();
                regionInputField.text = colorData.regions[i].name;
                regionName.transform.SetParent(panel.transform, false);
                RectTransform regionNameRect = regionName.transform.GetComponent<RectTransform>();
                regionNameRect.anchorMin = new Vector2(0, 0.5f);
                regionNameRect.anchorMax = new Vector2(0, 0.5f);
                regionNameRect.pivot = new Vector2(0, 0.5f);
                regionNameRect.anchoredPosition = new Vector2(5, 0);
                regionNameRect.sizeDelta = new Vector2(110, regionsPanel.sizeDelta.y / colorData.regions.Length);
                regionName.AddComponent<NameInputField>();

                GameObject textAreaRegionName = new GameObject("RegionNameTextArea");
                textAreaRegionName.AddComponent<RectMask2D>();
                RectTransform textAreaRectTransform = textAreaRegionName.GetComponent<RectTransform>();
                textAreaRegionName.transform.SetParent(regionName.transform, false);
                textAreaRectTransform.pivot = new Vector2(0.5f, 0.5f);
                textAreaRectTransform.anchorMin = Vector2.zero;
                textAreaRectTransform.anchorMax = Vector2.one;
                textAreaRectTransform.offsetMin = new Vector2(0, textAreaRectTransform.offsetMin.y);
                textAreaRectTransform.offsetMax = new Vector2(0, textAreaRectTransform.offsetMax.y);
                textAreaRectTransform.offsetMax = new Vector2(textAreaRectTransform.offsetMax.x, 0);
                textAreaRectTransform.offsetMin = new Vector2(textAreaRectTransform.offsetMin.x, 0);

                GameObject placeholderRegionName = new GameObject("RegionNamePlaceholder");
                placeholderRegionName.AddComponent<CanvasRenderer>();
                TextMeshProUGUI textMesProPlaceholder = placeholderRegionName.AddComponent<TextMeshProUGUI>();
                RectTransform placeholderRectTransform = placeholderRegionName.GetComponent<RectTransform>();
                placeholderRegionName.transform.SetParent(textAreaRegionName.transform, false);
                placeholderRectTransform.pivot = new Vector2(0.5f, 0.5f);
                placeholderRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                placeholderRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                placeholderRectTransform.sizeDelta = new Vector2(110, regionsPanel.sizeDelta.y / colorData.regions.Length);
                textMesProPlaceholder.alignment = TextAlignmentOptions.Center;
                textMesProPlaceholder.color = Color.gray;
                textMesProPlaceholder.fontSize = 12;
                textMesProPlaceholder.text = colorData.regions[i].name;

                GameObject textRegionName = new GameObject("RegionNameText");
                textRegionName.AddComponent<CanvasRenderer>();
                TextMeshProUGUI textMeshoProRegionName = textRegionName.AddComponent<TextMeshProUGUI>();
                RectTransform textRectTransform = textRegionName.GetComponent<RectTransform>();
                textRegionName.transform.SetParent(textAreaRegionName.transform, false);
                textRectTransform.pivot = new Vector2(0.5f, 0.5f);
                textRectTransform.anchorMin = Vector2.zero;
                textRectTransform.anchorMax = Vector2.one;
                textRectTransform.offsetMin = new Vector2(0, textRectTransform.offsetMin.y);
                textRectTransform.offsetMax = new Vector2(0, textRectTransform.offsetMax.y);
                textRectTransform.offsetMax = new Vector2(textRectTransform.offsetMax.x, 0);
                textRectTransform.offsetMin = new Vector2(textRectTransform.offsetMin.x, 0);
                textMeshoProRegionName.alignment = TextAlignmentOptions.Center;
                textMeshoProRegionName.color = Color.black;
                textMeshoProRegionName.fontSize = 12;

                regionInputField.textViewport = textAreaRectTransform;
                regionInputField.textComponent = textMeshoProRegionName;
                regionInputField.placeholder = textMesProPlaceholder;

                //second one is the height of the regions so a input field also
                GameObject regionHeight = new GameObject("RegionHeightInputField" + i);
                regionHeight.AddComponent<CanvasRenderer>();
                Image regionHeightImage = regionHeight.AddComponent<Image>();
                regionHeightImage.type = Image.Type.Sliced;
                regionHeightImage.fillCenter = true;
#if UNITY_EDITOR
                regionHeightImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/InputFieldBackground.psd");
#endif
                TMP_InputField regionHeightInputField = regionHeight.AddComponent<TMP_InputField>();
                regionHeightInputField.text = colorData.regions[i].height.ToString();
                regionHeight.transform.SetParent(panel.transform, false);
                RectTransform regionHeightNameRect = regionHeight.transform.GetComponent<RectTransform>();
                regionHeightNameRect.anchorMin = new Vector2(0.5f, 0.5f);
                regionHeightNameRect.anchorMax = new Vector2(0.5f, 0.5f);
                regionHeightNameRect.pivot = new Vector2(0.5f, 0.5f);
                regionHeightNameRect.anchoredPosition = new Vector2(-5, 0);
                regionHeightNameRect.sizeDelta = new Vector2(90, regionsPanel.sizeDelta.y / colorData.regions.Length);
                regionHeight.AddComponent<HeightInputField>();

                GameObject textAreaRegionHeight = new GameObject("RegionHeightTextArea");
                textAreaRegionHeight.AddComponent<RectMask2D>();
                RectTransform textAreaHeightRectTransform = textAreaRegionHeight.GetComponent<RectTransform>();
                textAreaRegionHeight.transform.SetParent(regionHeight.transform, false);
                textAreaHeightRectTransform.pivot = new Vector2(0.5f, 0.5f);
                textAreaHeightRectTransform.anchorMin = Vector2.zero;
                textAreaHeightRectTransform.anchorMax = Vector2.one;
                textAreaHeightRectTransform.offsetMin = new Vector2(0, textAreaHeightRectTransform.offsetMin.y);
                textAreaHeightRectTransform.offsetMax = new Vector2(0, textAreaHeightRectTransform.offsetMax.y);
                textAreaHeightRectTransform.offsetMax = new Vector2(textAreaHeightRectTransform.offsetMax.x, 0);
                textAreaHeightRectTransform.offsetMin = new Vector2(textAreaHeightRectTransform.offsetMin.x, 0);

                GameObject placeholderRegionHeight = new GameObject("RegionHeightPlaceholder");
                placeholderRegionHeight.AddComponent<CanvasRenderer>();
                TextMeshProUGUI textMesProPlaceholderHeight = placeholderRegionHeight.AddComponent<TextMeshProUGUI>();
                RectTransform placeholderRectTransformHeight = placeholderRegionHeight.GetComponent<RectTransform>();
                placeholderRegionHeight.transform.SetParent(textAreaRegionHeight.transform, false);
                placeholderRectTransformHeight.pivot = new Vector2(0.5f, 0.5f);
                placeholderRectTransformHeight.anchorMin = new Vector2(0.5f, 0.5f);
                placeholderRectTransformHeight.anchorMax = new Vector2(0.5f, 0.5f);
                placeholderRectTransformHeight.sizeDelta = new Vector2(110, regionsPanel.sizeDelta.y / colorData.regions.Length);
                textMesProPlaceholderHeight.alignment = TextAlignmentOptions.Center;
                textMesProPlaceholderHeight.color = Color.gray;
                textMesProPlaceholderHeight.fontSize = 12;
                textMesProPlaceholderHeight.text = colorData.regions[i].height.ToString();

                GameObject textRegionHeight = new GameObject("RegionHeightText");
                textRegionHeight.AddComponent<CanvasRenderer>();
                TextMeshProUGUI textMeshoProRegionHeight = textRegionHeight.AddComponent<TextMeshProUGUI>();
                RectTransform textRectTransformHeight = textRegionHeight.GetComponent<RectTransform>();
                textRegionHeight.transform.SetParent(textAreaRegionHeight.transform, false);
                textRectTransformHeight.pivot = new Vector2(0.5f, 0.5f);
                textRectTransformHeight.anchorMin = Vector2.zero;
                textRectTransformHeight.anchorMax = Vector2.one;
                textRectTransformHeight.offsetMin = new Vector2(0, textRectTransformHeight.offsetMin.y);
                textRectTransformHeight.offsetMax = new Vector2(0, textRectTransformHeight.offsetMax.y);
                textRectTransformHeight.offsetMax = new Vector2(textRectTransformHeight.offsetMax.x, 0);
                textRectTransformHeight.offsetMin = new Vector2(textRectTransformHeight.offsetMin.x, 0);
                textMeshoProRegionHeight.alignment = TextAlignmentOptions.Center;
                textMeshoProRegionHeight.color = Color.black;
                textMeshoProRegionHeight.fontSize = 12;

                regionHeightInputField.textViewport = textAreaHeightRectTransform;
                regionHeightInputField.textComponent = textMeshoProRegionHeight;
                regionHeightInputField.placeholder = textMesProPlaceholderHeight;

                GameObject regionColor = new GameObject("RegionColor" + i);
                regionColor.AddComponent<ColorImage>();
                ColorPicker regionColorPicker = regionColor.AddComponent<ColorPicker>();
                regionColor.AddComponent<Image>();
                regionColor.AddComponent<Button>();
                RectTransform regionColorRectTransform = regionColor.GetComponent<RectTransform>();
                regionColor.transform.SetParent(panel.transform, false);
                regionColorRectTransform.pivot = new Vector2(1f, 0.5f);
                regionColorRectTransform.anchorMin = new Vector2(1f, 0.5f);
                regionColorRectTransform.anchorMax = new Vector2(1f, 0.5f);
                regionColorRectTransform.anchoredPosition = new Vector2(-5, 0);
                regionColorRectTransform.sizeDelta = new Vector2(110, (regionsPanel.sizeDelta.y / colorData.regions.Length)-3);
                regionColorPicker.enabled = false;
            }
        }

        public void FallOnChanged()
        {
            terrainData.useFallon = useFallOnToggle.isOn;
            terrainGenerator.GenerateMap();
        }

        public void FallOffChanged()
        {
            terrainData.useFalloff = useFallOffToggle.isOn;
            terrainGenerator.GenerateMap();
        }

        public void FlatShadingChanged()
        {
            terrainData.useFlatShading = flatShadingToggle.isOn;
            terrainGenerator.GenerateMap();
        }

        public void HeightMeshMultiplierChanged()
        {
            terrainData.meshHeightMultiplier = heightMeshMultiplierSlider.value;
            terrainGenerator.GenerateMap();
        }

        public void OctavesChanged()
        {
            noiseData.octaves = (int) octavesSlider.value;
            terrainGenerator.GenerateMap();
        }

        public void PersistanceChanged()
        {
            noiseData.persistance = persistanceSlider.value;
            terrainGenerator.GenerateMap();
        }

        public void LacunarityChanged()
        {
            noiseData.lacunarity = lacunaritySlider.value;
            terrainGenerator.GenerateMap();
        }

        public void ScaleChanged()
        {
            noiseData.scale = (int) scaleSlider.value;
            terrainGenerator.GenerateMap();
        }

        public void SeedChanged()
        {
            noiseData.seed = (int) seedSlider.value;
            terrainGenerator.GenerateMap();
        }
    }
}