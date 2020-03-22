using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eCorpseSpawnMode
{
    None,
    Absorb,
    Destroy
}

public class CorpseSpawner : MonoBehaviour
{
    [SerializeField] CorpseData mData;

    bool mCorpseModeSet = false;

    int? corpse_count = null;
    public int? CorpseCount => corpse_count;

    public void SetCorpseMode(eCorpseSpawnMode mode, System.Action<GameObject> modeEvent)
    {
        if(!mCorpseModeSet)
        {
            mCorpseModeSet = true;
            mData.CurrentMode = mode;
        }

        mData.ModeEvent = modeEvent;
    }

    public void Spawn()
    {
        corpse_count = CorpseMgr.CreateCorpseOrNull(transform, mData).Length;
    }
}
