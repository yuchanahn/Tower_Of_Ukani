using UnityEditor;
using UnityEngine;

public class PixelLevelGenerator : MonoBehaviour {

	public Texture2D map;

	public ColorToPrefab[] colorMappings;

    private void Awake()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
	{
        for (int x = 0; x < map.width; x++)
		{
			for (int y = 0; y < map.height; y++)
			{
				GenerateTile(x, y);
			}
		}
	}

	private void GenerateTile(int x, int y)
	{
		Color pixelColor = map.GetPixel(x, y);
        if (pixelColor.a == 0)
            return;

        float offsetX = -(map.width / 2) + ((map.width % 2 == 0) ? 0.5f : 0);
        float offsetY = -(map.height / 2) + ((map.height % 2 == 0) ? 0.5f : 0);


        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(pixelColor))
            {
                Transform tile = Instantiate(colorMapping.prefab, transform).transform;
                tile.localPosition = new Vector2(x + offsetX, y + offsetY);
            }
        }
    }
}
