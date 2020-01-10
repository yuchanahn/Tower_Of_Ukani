using UnityEngine;
using Dongjun.Helper;

[System.Serializable]
public struct PlayerItemPickUpData
{
    [Header("Pick Up Data")]
    public float radius;
    public LayerMask layerMask;

    [Header("Inventory Ref")]
    public PlayerInventory inventory;
    public PlayerWeaponHotbar weaponHotbar;
    public PlayerActiveHotbar activeHotbar;
    public PlayerPassiveInventory passiveInventory;
}

public static class PlayerItemPickUp_Logic
{
    public static void PickUp(this PlayerItemPickUpData data, Transform tf)
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(tf.position, data.radius, data.layerMask);
        items.GetClosest<DroppedItem>(tf)?.OnPickUp(data);
    }
}
