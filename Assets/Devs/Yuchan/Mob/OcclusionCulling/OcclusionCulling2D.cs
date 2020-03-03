using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OcclusionCulling2D : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] float Range;
    [SerializeField] float CheckRate;
    float Range2;
    private void Awake()
    {
        Range2 = Range * Range;
        StartCoroutine(TimerOfCulling(CheckRate));
    }


    IEnumerator TimerOfCulling(float t)
    {
        while (true)
        {
            foreach (var i in (from mob in GetComponentsInChildren<Transform>(true)
                               where mob.GetComponent<Mob_Base>()
                               select mob).Select((tr, idx) => new
                               {
                                   tr,
                                   idx
                               }).GroupBy(m => (m.tr.position - Target.position).sqrMagnitude <= Range2, element => element.tr))
            {
                foreach (var ii in i)
                {
                    ii.gameObject.SetActive(i.Key);
                }
            }
            yield return new WaitForSeconds(t);
        }
    }
}
