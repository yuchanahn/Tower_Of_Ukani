using UnityEngine;
using UnityEngine.UI;

// Credits: https://forum.unity.com/threads/in-game-infinite-grids.366489/
// Modified by Dongjun

namespace Dongjun.LevelEditor
{
    public class GridDisplay : MonoBehaviour
    {
        [Header("Grid Line Matarial")]
        [SerializeField] private Material GLMat;

        [Header("Grid Info")]
        [SerializeField] private int gridCount = 5; //number of grids to draw on each side of the look position (half size)
        [SerializeField] private MapData mapData;

        [Header("Grid Color")]
        [SerializeField] private Color gridColor = Color.black;
        [SerializeField] private Color borderColor = Color.black;
        [SerializeField] private Slider gridOpacitySlider;

        private Camera cam;
        private Ray ray;
        private float rayDist;
        private Vector3 lookPosition;
        private Plane worldPlane = new Plane(Vector3.back, Vector3.zero); //world plane to draw the grid on

        public static GridDisplay Inst
        { get; private set; }

        private void Awake()
        {
            Inst = this;

            cam = GetComponent<Camera>();

            gridOpacitySlider.onValueChanged.AddListener((o) => { gridColor.a = o; });
            gridColor.a = gridOpacitySlider.value;
        }
        private void Update()
        {
            gridCount = Mathf.RoundToInt(cam.orthographicSize * 2);

            ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            worldPlane.Raycast(ray, out rayDist);
            lookPosition = ray.GetPoint(rayDist);
        }
        private void OnPostRender()
        {
            Vector3 roundedCamPos = new Vector3();
            roundedCamPos.x = Mathf.Round(lookPosition.x / mapData.CellSize);
            roundedCamPos.y = Mathf.Round(lookPosition.y / mapData.CellSize);

            GL.PushMatrix();

            // Set Material
            GLMat.SetPass(0);
            GL.Begin(GL.LINES);

            // Actual look position
            GL.Color(Color.black);
            GL.Vertex(lookPosition);
            GL.Vertex(lookPosition + Vector3.back);

            // Grid lines
            GL.Color(gridColor);
            for (int i = -gridCount + 1; i < gridCount + 1; i++)
            {
                //x lines
                GL.Vertex(roundedCamPos + new Vector3(i * mapData.CellSize, gridCount * mapData.CellSize));
                GL.Vertex(roundedCamPos + new Vector3(i * mapData.CellSize, -gridCount * mapData.CellSize));

                //y lines
                GL.Vertex(roundedCamPos + new Vector3(gridCount * mapData.CellSize, i * mapData.CellSize));
                GL.Vertex(roundedCamPos + new Vector3(-gridCount * mapData.CellSize, i * mapData.CellSize));
            }

            // Grid Border
            GL.Color(borderColor);
            Vector2 gridBorder = new Vector2(mapData.MaxSizeX * mapData.CellSize, mapData.MaxSizeY * mapData.CellSize);
            GL.Vertex(new Vector3(gridBorder.x, gridBorder.y));
            GL.Vertex(new Vector3(gridBorder.x, 0));
            GL.Vertex(new Vector3(gridBorder.x, 0));
            GL.Vertex(new Vector3(0, 0));
            GL.Vertex(new Vector3(0, 0));
            GL.Vertex(new Vector3(0, gridBorder.y));
            GL.Vertex(new Vector3(0, gridBorder.y));
            GL.Vertex(new Vector3(gridBorder.x, gridBorder.y));
            GL.End();

            GL.PopMatrix();
        }

        public Vector2Int WorldToGrid(Vector2 pos)
        {
            return new Vector2Int(Mathf.FloorToInt(pos.x) / mapData.CellSize, Mathf.FloorToInt(pos.y) / mapData.CellSize);
        }
        public Vector2 GridToWorld(Vector2Int pos)
        {
            return new Vector2(pos.x * mapData.CellSize, pos.y * mapData.CellSize) + (Vector2.one * (mapData.CellSize * 0.5f));
        }
    }
}
