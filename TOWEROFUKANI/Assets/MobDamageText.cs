using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MobDamageText : Object_ObjectPool<MobDamageText>
{
    [SerializeField] Text mText;
    [SerializeField] float DestoryT;
    Vector2 mPoint;
    [Disable] public string Damage;
   

    public static void Show(int dmg, Vector2 point)
    {
        var Obj = ObjectPool.createUI(ID, Camera.main.WorldToScreenPoint(point));
        Obj.GetComponent<MobDamageText>().mText.text = dmg.ToString();
        Obj.GetComponent<MobDamageText>().mPoint = point;
    }

    public override void ThisStart()
    {
        Damage = mText.text;
        mText.color = new Color(0,0,0,1);
        transform.localScale = new Vector3(1,1,1);
        DestroyObj(DestoryT);
    }

    private void Update()
    {
        transform.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(mPoint);
        transform.localPosition += Vector3.up * Time.deltaTime * 0.1f;
        transform.localScale -= (Vector3)Vector2.one * Time.deltaTime * 0.1f;
        mText.color -= new Color(0,0,0,Time.deltaTime * 0.5f);
    }
}
