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
    public static void PickUp(this PlayerItemPickUpData data, Transform tf, bool input)
    {
        if (!input) return;

        Collider2D[] items = Physics2D.OverlapCircleAll(tf.position, data.radius, data.layerMask);
        items.GetClosest<DroppedWeapon>(tf)?.OnPickUp();
    }
}
