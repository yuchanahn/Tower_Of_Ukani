using System;
using UnityEngine;
using UnityEngine.Events;


interface IVSForEach
{
    Action<GameObject> func();
}
public class VSForEach : MonoBehaviour
{
    [SerializeField] Transform Ranges;
    [SerializeField] GameObject ev;

    public void run()
    {
        foreach (var ii in Ranges.GetComponentsInChildren<Transform>())
        {
            ev.GetComponent<IVSForEach>().func()(ii.gameObject);
        }
    }
}