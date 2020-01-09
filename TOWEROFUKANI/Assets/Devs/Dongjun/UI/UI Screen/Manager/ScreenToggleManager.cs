using UnityEngine;

public class ScreenToggleManager : MonoBehaviour
{
    [SerializeField] private UI_Screen inventory;

    void Update()
    {
        if (Input.GetKeyDown(PlayerUIKeys.InventoryToggle))
            inventory.Toggle();
    }
}
