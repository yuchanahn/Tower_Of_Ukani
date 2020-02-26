using UnityEditor;
using UnityEngine;
using Dongjun.Helper;
using System.Collections.Generic;

public class PixelLevelGenerator : MonoBehaviour
{
    [SerializeField] private Texture2D mapTexture;
    [SerializeField] private SpriteRenderer mapTempSprite;
    [SerializeField] private ColorToPrefab[] colorMappings;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private int GridSize = 1;
    [SerializeField] GameObject prefab;

    private void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        for (int x = 0; x < mapTexture.width; x++)
        {
            for (int y = 0; y < mapTexture.height; y++)
            {
                GenerateTile(x, y);
            }
        }

        mapTempSprite.sprite = null;
        GM.CurMapName = transform.root.name;
        GM.MapSize[GM.CurMapName] = mapTexture;
        GM.CurMapCenter = transform.position;
    }
    [SerializeField] int map_w;
    [SerializeField] int map_h;
    [SerializeField] int[] objSize;
    private void GenerateTile(int x, int y)
    {
        Color pixelColor = mapTexture.GetPixel(x, y);
        if (pixelColor.a == 0)
            return;

        float offsetX = -(mapTexture.width / 2) + ((mapTexture.width % 2 == 0) ? 0.5f : 0);
        float offsetY = -(mapTexture.height / 2) + ((mapTexture.height % 2 == 0) ? 0.5f : 0);

        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(pixelColor))
            {
                Transform tile = Instantiate(colorMapping.prefab, transform).transform;
                tile.localPosition = new Vector2(x + offsetX, y + offsetY);

                if (((1 << colorMapping.prefab.layer) & GroundLayer) != 0)
                {
                    foreach (var osize in objSize)
                    {
                        for (int i = 0; i < osize; i++)
                        {
                            for (int j = 0; j < osize; j++)
                            {
                                var lpfs = tile.position.Add(x: i, y: -j);
                                var __x = (tile.localPosition.x - offsetX) + i;
                                var __y = (tile.localPosition.y - offsetY) - j;
                                if (__x >= 0 && __x < map_w && __y >= 0 && __y < map_h)
                                    GridView.Inst[osize].GetNodeAtWorldPostiton(lpfs).isObstacle = true;
                            }
                        }
                    }
                }
            }
        }
    }
}


