using UnityEngine;

public class PistolItem : GunItem
{
    protected override void Awake()
    {
        base.Awake();

        shootTimer.StartAsEnded = true;
        shootTimer.EndTime = new FloatStat(0.15f, min: 0.01f);
        reloadTimer.EndTime = new FloatStat(0.5f, min: 0.01f);
        swapMagazineTimer.EndTime = new FloatStat(0.8f, min: 0.01f);

        magazineSize = new IntStat(6, min: 0);
    }
}
