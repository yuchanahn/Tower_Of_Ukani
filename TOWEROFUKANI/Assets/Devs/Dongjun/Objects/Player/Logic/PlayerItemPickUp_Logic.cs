using UnityEngine;

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
        if (items.Length == 0) return;

        // Get Closest Item
        DroppedWeapon droppedItem = null;
        float dist = -1;

        for (int i = 0; i < items.Length; i++)
        {
            if (dist == -1 || dist > Vector2.Distance(items[i].transform.position, tf.position))
                droppedItem = items[i].GetComponent<DroppedWeapon>();
        }

        droppedItem?.OnPickUp();
    }
}
