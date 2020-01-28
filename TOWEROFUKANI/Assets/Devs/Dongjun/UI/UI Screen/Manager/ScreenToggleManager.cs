using UnityEngine;

public class ScreenToggleManager : MonoBehaviour
{
    [SerializeField] private UI_Screen inventory;
    [SerializeField] private UI_Screen activeHotbar;
    [SerializeField] private UI_Screen consumableHotbar;

    void Update()
    {
        if (Input.GetKeyDown(PlayerUIKeys.InventoryToggle))
            inventory.Toggle();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool tempActive = activeHotbar.gameObject.activeSelf;
            activeHotbar.gameObject.SetActive(!activeHotbar.gameObject.activeSelf);
            consumableHotbar.gameObject.SetActive(tempActive);
        }
    }
}
