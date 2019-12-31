using UnityEditor;
using UnityEngine;
using Dongjun.Helper;

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
                    var p = GridView.Inst.GetNodeAtWorldPostiton(tile.position).pos;

                    GridView.Inst.GetNodeAtWorldPostiton(tile.position.Add(x: -0.25f, y: -0.25f)).isObstacle = true;
                    GridView.Inst.GetNodeAtWorldPostiton(tile.position.Add(x: 0.25f, y: 0.25f)).isObstacle = true;
                    GridView.Inst.GetNodeAtWorldPostiton(tile.position.Add(x: -0.25f, y: 0.25f)).isObstacle = true;
                    GridView.Inst.GetNodeAtWorldPostiton(tile.position.Add(x: 0.25f, y: -0.25f)).isObstacle = true;
                }
            }
            
        }

        /*
            if(((1<<other.gameObject.layer) & includeLayers) != 0)
            {
                //It matched one
            } 
         */

    }
}


