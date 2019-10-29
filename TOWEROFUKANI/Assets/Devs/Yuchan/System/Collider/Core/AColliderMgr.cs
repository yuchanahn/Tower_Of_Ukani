using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AColliderMgr : MonoBehaviour
{
    BoxCollider2D box2;
    Vector2 mOffSet;
    [SerializeField] LayerMask TargetLayer;
    private void Awake()
    {
        box2 = GetComponent<BoxCollider2D>();
        mOffSet = box2.offset;
    }

    public Vector2 OffSet => mOffSet;


    public void ReStart(Vector2 OffSet)
    {
        box2.offset = OffSet;
        enabled = true;
    }

    public void Stop()
    {
        enabled = false;
    }



    void Update()
    {
        var Objs = Physics2D.OverlapBoxAll((Vector2)transform.position + box2.offset, box2.size, 0, TargetLayer);
        if (Objs.Length > 0)
        {
            GetComponentInParent<AColliderEvent>().RecvEvent(Objs);
        }
    }
}
