

using UnityEngine;

internal interface IObjectPool
{
    GameObject CreateThis(Vector2 pos);
    void Init(Vector2 pos);
}