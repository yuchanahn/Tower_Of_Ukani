using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHPBar : Object_ObjectPool<MobHPBar>
{
    [SerializeField] AUI_Slider mHpBar;
    [SerializeField] Transform mLook;
    [SerializeField] AStat mStat;
    [SerializeField] float DestoryT;

    static Dictionary<GameObject, MobHPBar> mobHPBars = new Dictionary<GameObject, MobHPBar>();
    
    [HideInInspector]
    public float sizeY;

    public override void ThisStart()
    {
        DestroyObj(DestoryT);
    }

    public static void Show(GameObject Mob, Vector2 mobPos, float sizeY)
    {
        if (mobHPBars.ContainsKey(Mob))
        {
            mobHPBars[Mob].SetOnUI(mobPos);
            return;
        }

        MobHPBar obj = ObjectPool.createUI(ID, Vector2.zero).GetComponent<MobHPBar>();
        obj.SetStat(Mob.GetComponent<AStat>(), Mob.transform);
        obj.sizeY = sizeY;
        mobHPBars[Mob] = obj;
    }

    public override void SetOff()
    {
        base.SetOff();
        if(mLook) mobHPBars.Remove(mLook.gameObject);
    }

    public void SetStat(AStat stat, Transform look)
    {
        mLook = look;
        mStat = stat;
    }
    private void Update()
    {
        if (mLook)
        {
            (transform as RectTransform).position = Camera.main.WorldToScreenPoint(mLook.position + Vector3.up * ((sizeY / 2) + 1f));

            mHpBar.CUR_ = mStat.HP;
            mHpBar.MAX_ = mStat.MAXHP;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnHPZero()
    {
        DestroyObj();
    }
}
