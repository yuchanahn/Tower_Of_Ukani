using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseMgr : MonoBehaviour
{
    public static GameObject[] CreateCorpseOrNull(Transform __mobPos, CorpseData corpseData)
    {
        if (corpseData.Count == 0) return null;
        var corpse = corpseData.mCorpse;

        Vector2 mobPos = __mobPos.position;
        Vector2 size = corpse.GetComponent<BoxCollider2D>().size;

        size.x *= corpse.transform.localScale.x;
        size.y *= corpse.transform.localScale.y;

        int MobCount = corpseData.Count;

        GameObject[] gObj = new GameObject[MobCount];

        for (int i = 0; i < MobCount; i++)
        {
            var w = Random.insideUnitCircle;
            gObj[i] = corpse.Spawn(mobPos + w * 0.3f).gameObject;
            gObj[i].GetComponent<Corpse>().Init(corpseData);
        }

        return gObj;
    }
}
