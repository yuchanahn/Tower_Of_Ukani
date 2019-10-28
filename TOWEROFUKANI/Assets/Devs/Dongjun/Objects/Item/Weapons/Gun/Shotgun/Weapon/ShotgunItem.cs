using System.Collections;
using UnityEngine;

public class ShotgunItem : GunItem
{
    protected override void Awake()
    {
        base.Awake();

        shootTimer.StartAsEnded = true;
        shootTimer.EndTime = new FloatStat(0.1f, min: 0.01f);
        reloadTimer.EndTime = new FloatStat(0.12f, min: 0.01f);
        swapMagazineTimer.EndTime = new FloatStat(1f, min: 0.01f);

        magazineSize = new IntStat(20, min: 0);
    }
}
