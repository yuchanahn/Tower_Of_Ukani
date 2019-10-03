

using UnityEngine;

[System.Serializable]
public struct GunData
{
    [Header("Timer")]
    public TimerData shootTimer;
    public TimerData reloadTimer;
    public TimerData swapMagazineTimer;

    [Header("Ammo")]
    public int magazineSize;
    public int loadedBullets;
    public bool isBulletLoaded;
}