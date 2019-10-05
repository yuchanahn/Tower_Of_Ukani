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
		{
			// The pixel is transparrent. Let's ignore it!
			return;
		}

		foreach (ColorToPrefab colorMapping in colorMappings)
		{
			if (colorMapping.color.Equals(pixelColor))
			{
				Transform tile = Instantiate(colorMapping.prefab, transform).transform;
                tile.localPosition = new Vector2(x - (map.width / 2), y - (map.height / 2));
            }
		}
	}
}

//[CustomEditor(typeof(PixelLevelGenerator))]
//public class PixelLevelGeneratorEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        if (GUILayout.Button("Generate Level!"))
//        {
//            Debug.Log("Pressed");
//            PixelLevelGenerator generator = (PixelLevelGenerator)target;
//            generator.GenerateLevel();
//        }

//        base.OnInspectorGUI();
//    }
//}
