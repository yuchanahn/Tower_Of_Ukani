using UnityEngine;

public class CommonObjs : MonoBehaviour
{
    public static CommonObjs Inst;

    [SerializeField] private Camera mainCam;
    [SerializeField] private CameraShake camShake;
    [SerializeField] private Rigidbody2D playerRB2D;

    public Camera MainCam => mainCam;
    public CameraShake CamShake => camShake;
    public Rigidbody2D PlayerRB2D => playerRB2D;

    private void Awake()
    {
        Inst = this;
    }
}