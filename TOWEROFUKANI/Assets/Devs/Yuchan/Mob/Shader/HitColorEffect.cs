using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitColorEffect : MonoBehaviour
{
    [SerializeField] Material Material;
    [SerializeField] float Time;
    [SerializeField] SpriteRenderer[] SpriteRenderers;
    Material[] Origin_Material;

    private void Awake()
    {
        Origin_Material = new Material[SpriteRenderers.Length];
        for (int i =0; i < SpriteRenderers.Length; i++)
        {
            Origin_Material[i] = SpriteRenderers[i].material;
        }
    }
    public void OnHit()
    {
        for (int i = 0; i < SpriteRenderers.Length; i++)
        {
            SpriteRenderers[i].material = Material;
        }
        ATimer.SetAndReset(GetInstanceID()+"HitColorEffect", Time, ()=> {
            for (int i = 0; i < SpriteRenderers.Length; i++)
            {
                SpriteRenderers[i].material = Origin_Material[i];
            }
        });
    }

    private void OnDestroy()
    {
        ATimer.Pop(GetInstanceID() + "HitColorEffect");
    }
}
