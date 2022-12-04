using UnityEngine;
using System;
using System.Text.RegularExpressions;

namespace ElectorchStrauss.ProceduralTerrainSystem.Scripts
{
    public class ColorPicker : MonoBehaviour
    {
        [SerializeField] private Texture2D colorPicker;
        private int ImageWidth = 400;
        private int ImageHeight = 400;
        private ColorData colorData;
        private MapGenerator terrainGenerator;
        private Rect rect;
        private Vector2 localPoint;
        private Rect r;
        private int px, py;
        private Color col;
        public Vector2 oofset = new Vector2(-35,0);
        private void Start()
        {
            terrainGenerator = GameObject.Find("TerrainGenerator").GetComponent<MapGenerator>();
            colorData = transform.root.GetComponent<TerrainRuntimeModification>().colorData;
            colorPicker = Resources.Load<Texture2D>("ColorPicker");
            rect = new Rect(Screen.width - ImageWidth, Screen.height - ImageHeight, ImageWidth, ImageHeight);
        }

        void OnGUI()
        {
            if (GUI.RepeatButton(rect, colorPicker))
            {
                Vector2 pickpos = Event.current.mousePosition;
                
                float aaa  = pickpos.x - ImageWidth +oofset.x;

                float bbb  =  pickpos.y - ImageHeight +oofset.y;

                int aaa2  = (int)((Screen.width - ImageWidth + aaa) * (colorPicker.width / (ImageWidth-0f)));

                int bbb2  =  (int)((Screen.height - ImageHeight - bbb) * (colorPicker.height / (ImageHeight-0f)));
                col = colorPicker.GetPixel(aaa2, bbb2);
                string resultString = Regex.Match(transform.name, @"\d+").Value;
                int value = int.Parse(resultString);
                colorData.regions[value].colour = col;
                terrainGenerator.GenerateMap();
            }
        }
    }
}