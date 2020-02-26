using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eOverlapDamageType
{
    Instant,
    OverTime,
}

public class Mob_OverlapDamage : MonoBehaviour
{
    [SerializeField] AStat Stat;
    [SerializeField] float DelayT;

    [SerializeField] bool mOverlapAble = true;


    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (mOverlapAble)
            {
                OnOverlap();
                mOverlapAble = false;
                ATimer.Set(GetInstanceID() + "OnOverlap", DelayT, () => { mOverlapAble = true; });
            }
        }
    }

    private void OnDestroy()
    {
        ATimer.Pop(GetInstanceID() + "OnOverlap");
    }

    void OnOverlap()
    {
        PlayerStats.Inst.Damage(Stat.Damage);
    }   
}
