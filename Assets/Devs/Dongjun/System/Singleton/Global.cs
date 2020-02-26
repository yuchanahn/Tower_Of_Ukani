using UnityEngine;

public class Global : SingletonBase<Global>
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private CameraShake camShake;

    public Camera MainCam => mainCam;
    public CameraShake CamShake => camShake;
}