using UnityEngine;

public class Global : SingletonBase<Global>
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private CameraShake camShake;
    [SerializeField] private Rigidbody2D playerRB2D;

    public Camera MainCam => mainCam;
    public CameraShake CamShake => camShake;
    public Rigidbody2D PlayerRB2D => playerRB2D;
}