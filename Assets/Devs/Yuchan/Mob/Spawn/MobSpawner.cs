using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnData
{
    public Mob_Base mob;
    public float mob_spawn_rate;
    public AnimationCurve start_jump;
    public Transform start_dir;
}


public class MobSpawner : MonoBehaviour
{
    public SpawnData[] table;
    Vector2 dir(Transform d) => (d.position - transform.position).normalized;


    private void Start()
    {
        table.for_each(x => start_spawn(x));
    }
    private void OnDestroy()
    {
        table.for_each(d => ATimer.Pop("" + GetInstanceID() + d.GetHashCode()));

    }

    void start_spawn(SpawnData d)
    {
        StatusEffect_Knokback.Create(Instantiate(d.mob.gameObject, transform.position, Quaternion.identity), dir(d.start_dir), d.start_jump);
        StartCoroutine(one_frame(() => ATimer.SetAndReset("" + GetInstanceID() + d.GetHashCode(), d.mob_spawn_rate, () => start_spawn(d))));
    }


    IEnumerator one_frame(System.Action act)
    {
        yield return new WaitForEndOfFrame();
        act();
    }
}
