using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class WeighObject : MonoBehaviour
{
    [SerializeField]LayerMask checkLayer;

    float width = 0;
    float height = 0;

    float mass;
    float gravityScale;

    private void Awake()
    {
        Init();
    }

    // 변수 초기화
    void Init()
    {
        Rigidbody2D _rg = GetComponent<Rigidbody2D>();
        BoxCollider2D _bc = GetComponent<BoxCollider2D>();
        mass = _rg.mass;
        gravityScale = _rg.gravityScale;
        width = _bc.size.x * transform.lossyScale.x;
        height = _bc.size.y * transform.lossyScale.y;
    }


    public float GetDownForce(List<WeighObject> checkedArr = null)
    {
        float downForce = 0;

        if(checkedArr == null)
        {
            checkedArr = new List<WeighObject>();
        }

        checkedArr.Add(this);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(width, height), 0, Vector2.up, 0.1f, checkLayer);


            var check =
            from hit in hits
            //위에있는지
            where (hit.transform.position.Foot(new Vector2(0, height)).y > transform.position.y)
            //이미 확인 했는지
            where (!checkedArr.Exists(a => a.gameObject == hit.transform.gameObject))
            //이 컴포넌트 달려 있는지
            where (hit.transform.GetComponent<WeighObject>())
            //y속도 0인지
            //where (Mathf.Abs(hit.transform.GetComponent<Rigidbody2D>().velocity.y) < 0.1f)
            select hit;

        foreach(RaycastHit2D hit in check)
        {
            if (hit.transform.GetComponent<OBB_Player>() == null && hit.transform.GetComponent<Mob_Base>() == null)
                downForce += hit.transform.GetComponent<WeighObject>().GetDownForce(checkedArr);
            else
            {
                downForce += 100;
                checkedArr.Add(hit.transform.GetComponent<WeighObject>());
            }
        }

        downForce += (gravityScale * mass);

        return downForce;
    }
}
