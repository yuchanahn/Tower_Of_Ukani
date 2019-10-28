using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : MonoBehaviour
{
    #region Var: Inspector
    [SerializeField] private Text nameText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Image[] weaponIcon = new Image[3];
    [SerializeField] private WeaponItem[] weapons = new WeaponItem[3];
    #endregion

    #region Var: Weapon Holder
    private int currentSlot = 0;
    private WeaponItem oldWeapon;
    private WeaponItem curWeapon;
    #endregion


    #region Method Unity
    private void Awake()
    {
        if (weapons is null)
            return;

        // Init Selected Weapon
        curWeapon = weapons[0];
        curWeapon.SelectWeapon(true);

        for (int i = 0; i < weaponIcon.Length; i++)
        {
            Inventory.Inst.AddItem(weapons[i]); // Test
            weaponIcon[i].sprite = weapons[i].Info.Icon;
        }
    }
    private void Update()
    {
        if (weapons is null)
            return;

        GetInput();
        ChangeWeapon();
        UpdateUI();
    }
    #endregion

    #region Method: Change Weapon
    private void GetInput()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            if (scroll < 0)
                currentSlot = currentSlot == weapons.Length - 1 ? 0 : currentSlot + 1;
            else
                currentSlot = currentSlot == 0 ? weapons.Length - 1 : currentSlot - 1;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentSlot = currentSlot == weapons.Length - 1 ? 0 : currentSlot + 1;
        }
    }
    private void ChangeWeapon()
    {
        if (oldWeapon == weapons[currentSlot])
            return;

        oldWeapon?.SelectWeapon(false);
        curWeapon = weapons[currentSlot];
        curWeapon.SelectWeapon(true);
        oldWeapon = curWeapon;
    }
    #endregion

    #region Method: UI
    private void UpdateUI()
    {
        // Show Name
        nameText.text = curWeapon.Info.Name;

        // Show Weapon Info
        if (curWeapon is GunItem)
        {
            GunItem gun = curWeapon as GunItem;
            ammoText.text = $"{gun.loadedBullets} / {gun.magazineSize.Value}";
        }
        else
        {
            ammoText.text = string.Empty;
        }
    }
    #endregion
}
