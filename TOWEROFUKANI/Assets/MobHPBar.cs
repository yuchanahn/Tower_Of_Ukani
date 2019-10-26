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
        var obj = ObjectPool.createUI(ID, Camera.main.WorldToScreenPoint(mobPos) + new Vector3(0, (sizeY / 2) + 1f));
        obj.GetComponent<MobHPBar>().SetStat(Mob.GetComponent<AStat>(), Mob.transform);
        mobHPBars[Mob] = obj.GetComponent<MobHPBar>();
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
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(mLook.position + Vector3.up * 1f);

        mHpBar.CUR_ = mStat.HP;
        mHpBar.MAX_ = mStat.MAXHP;
    }

    public void OnHPZero()
    {
        DestroyObj();
    }
}
