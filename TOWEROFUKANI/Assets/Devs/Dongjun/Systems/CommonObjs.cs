using UnityEngine;

public class CommonObjs : MonoBehaviour
{
    public static CommonObjs Inst;

    [SerializeField] private Camera mainCam;
    [SerializeField] private CameraShake camShake;

    public Camera MainCam => mainCam;
    public CameraShake CamShake => camShake;

    private void Awake()
    {
        Inst = this;
    }
}