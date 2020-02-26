using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledSpriteOffsetController : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.5f;
    SpriteRenderer rend;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        rend.material.SetVector("_Offset", new Vector4(0, offset));
    }
}

