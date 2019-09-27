using UnityEngine;

public class PixelCamera : MonoBehaviour
{
    [SerializeField] int pixelPerUnit = 16;
    [SerializeField] Vector2Int resolution = new Vector2Int(1920, 1080);
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform pixelatedQuad;

    // Main Camera's Values
    private float mainOrthoSize;
    private float mainCamSizeY;
    private float mainCamSizeX;

    // Pixel Camera's Values
    private Camera pixelCamera;
    private int pixelRender_Width;
    private int pixelRender_Height;

    protected void Awake()
    {
        pixelCamera = GetComponent<Camera>();

        RenderTexture pixelatedRenderTex = pixelCamera.targetTexture;
        pixelatedRenderTex.width = resolution.x;
        pixelatedRenderTex.height = resolution.y;
    }
    void LateUpdate()
    {
        // Set Pixel Camera's Aspect Ratio
        pixelCamera.aspect = mainCamera.aspect;

        // Get Main Camera's Orthographic Size
        mainOrthoSize = mainCamera.orthographicSize;

        // Set Pixel Camera's Orthographic Size
        pixelCamera.orthographicSize = mainOrthoSize;

        // Get Render Size
        mainCamSizeX = mainOrthoSize * 2 * Screen.width / Screen.height;
        mainCamSizeY = mainOrthoSize * 2;

        // Set Pixel Camera's Width and Height
        pixelRender_Width = Mathf.RoundToInt(mainCamSizeX * pixelPerUnit);
        pixelRender_Height = Mathf.RoundToInt(mainCamSizeY * pixelPerUnit);

        // Set Pixelated Quad's Scale
        pixelatedQuad.localScale = new Vector2(mainCamSizeX, mainCamSizeY);

        // Set Position
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, transform.position.z);
        pixelatedQuad.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, pixelatedQuad.position.z);
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Point;

        RenderTexture buffer = RenderTexture.GetTemporary(pixelRender_Width, pixelRender_Height);
        buffer.filterMode = FilterMode.Point;

        Graphics.Blit(source, buffer);
        Graphics.Blit(buffer, destination);

        RenderTexture.ReleaseTemporary(buffer);
    }
}