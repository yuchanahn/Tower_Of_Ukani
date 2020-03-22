using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseMgr : MonoBehaviour
{
    public static GameObject[] CreateCorpseOrNull(Transform __mobPos, CorpseData corpseData)
    {
        if (corpseData.SpawnCount == 0) return null;
        var corpse = corpseData.Corpse;

        Vector2 mobPos = __mobPos.position;
        Vector2 size = corpse.GetComponent<BoxCollider2D>().size;

        size.x *= corpse.transform.localScale.x;
        size.y *= corpse.transform.localScale.y;

        int MobCount = corpseData.SpawnCount;

        GameObject[] gObj = new GameObject[MobCount];

        for (int i = 0; i < MobCount; i++)
        {
            var w = Random.insideUnitCircle;

            var spawnedCorpse = corpse.Spawn(mobPos + w * 0.3f);
            spawnedCorpse.Init(corpseData);

            gObj[i] = spawnedCorpse.gameObject;
        }

        return gObj;
    }
}
