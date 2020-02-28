using UnityEngine;

namespace Dongjun.LevelEditor
{
    public class CamController : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed = 1f;
        [SerializeField] private float minScroll = 4f;
        [SerializeField] private float maxScroll = 30f;

        private Vector3 mouseDownPos;

        public Camera cam
        { get; private set; }
        public bool IsPanning
        { get; private set; }

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        public void Scroll()
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
        public void Pan()
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                IsPanning = false;
                mouseDownPos = cam.ScreenToWorldPoint(Input.mousePosition);
                return;
            }

            IsPanning = true;

            if (Input.GetKeyDown(KeyCode.Mouse0))
                mouseDownPos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetKey(KeyCode.Mouse0))
                transform.position += mouseDownPos - cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}

