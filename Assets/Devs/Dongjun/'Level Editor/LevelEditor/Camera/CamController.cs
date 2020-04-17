using UnityEngine;

namespace Dongjun.LevelEditor
{
    public class CamController : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed = 1f;
        [SerializeField] private float minScroll = 4f;
        [SerializeField] private float maxScroll = 30f;

        private MapData mapData;
        private Vector3 mouseDownPos;

        public Camera cam
        { get; private set; }
        public bool IsPanning
        { get; private set; }

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        public void Init(MapData mapData)
        {
            this.mapData = mapData;
            transform.position = new Vector3(mapData.MaxSizeX * 0.5f, mapData.MaxSizeY * 0.5f, transform.position.z);
        }

        private void ToCenter()
        {
            if (Input.GetKeyDown(KeyCode.C))
                transform.position = new Vector3(mapData.MaxSizeX * 0.5f, mapData.MaxSizeY * 0.5f, transform.position.z);
        }
        private void Scroll()
        {
            if (Input.mouseScrollDelta.y == 0)
                return;

            if (Input.mouseScrollDelta.y > 0)
                cam.orthographicSize -= scrollSpeed * cam.orthographicSize * Time.deltaTime;
            else
                cam.orthographicSize += scrollSpeed * cam.orthographicSize * Time.deltaTime;

            // Clamp
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minScroll, maxScroll);
        }
        private void Pan()
        {
            if (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Mouse2))
            {
                IsPanning = false;
                mouseDownPos = cam.ScreenToWorldPoint(Input.mousePosition);
                return;
            }

            IsPanning = true;

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse2))
                mouseDownPos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse2))
                transform.position += mouseDownPos - cam.ScreenToWorldPoint(Input.mousePosition);
        }

        public void UserControl()
        {
            ToCenter();
            Scroll();
            Pan();
        }
    }
}

