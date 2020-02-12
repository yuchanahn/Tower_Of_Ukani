using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MobDamageText : Object_ObjectPool<MobDamageText>
{
    [SerializeField, Range(0, 5)] float JumpPower;

    [SerializeField, Range(0, 5)] float RandomPosRange;

    [SerializeField] public Text mText;
    [SerializeField] float DestoryT;
    Vector2 mPoint;
    Vector2 mOriginPoint;
    Vector2 RandPos;
    /*[Disable]*/ public string Damage;
   

    public static MobDamageText Show(int dmg, Vector2 point)
    {
        var Obj = ObjectPool.createUI(ID, Camera.main.WorldToScreenPoint(point));

        var MobDamageText = Obj.GetComponent<MobDamageText>();

        MobDamageText.mText.text = dmg.ToString();
        MobDamageText.RandPos = (Random.insideUnitCircle * MobDamageText.RandomPosRange);
        MobDamageText.mOriginPoint = point + MobDamageText.RandPos;
        return Obj.GetComponent<MobDamageText>();
    }

    public void SetPoint(Vector2 point)
    {
        mOriginPoint = point + RandPos;
    }

    public override void Begin()
    {
        Damage = mText.text;
        mPoint = mOriginPoint;

        Color newColor = mText.color;
        newColor.a = 1;
        mText.color = newColor;

        transform.localScale = new Vector3(1,1,1);
        DestroyObj(DestoryT);
    }

    private void Update()
    {
        transform.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(mPoint);
        mPoint += Vector2.up * Time.deltaTime * JumpPower;
        //transform.localScale -= (Vector3)Vector2.one * Time.deltaTime * JumpPower;

        Color newColor = mText.color;
        newColor.a -= Time.deltaTime / DestoryT;
        mText.color = newColor;
    }
}
