using UnityEngine;
using Dongjun.Helper;

[System.Serializable]
public struct PlayerItemPickUpData
{
    public float radius;
    public LayerMask layerMask;
}

public static class PlayerItemPickUp_Logic
{
    public static void PickUp(this PlayerItemPickUpData data, Transform tf)
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(tf.position, data.radius, data.layerMask);
        items.GetClosest<DroppedItem>(tf)?.OnPickUp();
    }
}
