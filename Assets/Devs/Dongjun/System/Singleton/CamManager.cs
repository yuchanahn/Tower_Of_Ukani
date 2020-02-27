using UnityEngine;

public class CamManager : SingletonBase<CamManager>
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private CameraShake camShake;

    public Camera MainCam => mainCam;
    public CameraShake CamShake => camShake;
}