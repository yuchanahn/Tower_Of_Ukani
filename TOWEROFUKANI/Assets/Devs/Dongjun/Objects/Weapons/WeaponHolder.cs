using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : MonoBehaviour
{
    #region Var: Inspector
    [SerializeField] private Text nameText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Weapon[] weaponSlot = new Weapon[3];
    #endregion

    #region Var: Current Weapon
    private Weapon curWeapon;
    #endregion


    #region Method Unity
    private void Awake()
    {
        if (weaponSlot == null)
            return;

        curWeapon = weaponSlot[0];
        curWeapon.SelectWeapon(true);
    }
    private void Update()
    {
        if (weaponSlot == null)
            return;

        ChangeWeapon();
        UpdateUI();
    }
    #endregion

    #region Method: Change Weapon
    private void SetWeapon(Weapon weapon)
    {
        curWeapon.SelectWeapon(false);
        curWeapon = weapon;
        curWeapon.SelectWeapon(true);
    }
    private void ChangeWeapon()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            if (curWeapon == weaponSlot[0])
                SetWeapon(weaponSlot[1]);
            else if (curWeapon == weaponSlot[1])
                SetWeapon(weaponSlot[2]);
            else if (curWeapon == weaponSlot[2])
                SetWeapon(weaponSlot[0]);
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            if (curWeapon == weaponSlot[0])
                SetWeapon(weaponSlot[2]);
            else if (curWeapon == weaponSlot[1])
                SetWeapon(weaponSlot[0]);
            else if (curWeapon == weaponSlot[2])
                SetWeapon(weaponSlot[1]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && curWeapon != weaponSlot[0])
            SetWeapon(weaponSlot[0]);
        else if (Input.GetKeyDown(KeyCode.Alpha2) && curWeapon != weaponSlot[1])
            SetWeapon(weaponSlot[1]);
        else if (Input.GetKeyDown(KeyCode.Alpha3) && curWeapon != weaponSlot[2])
            SetWeapon(weaponSlot[2]);
    }
    #endregion

    #region Method: UI
    private void UpdateUI()
    {
        // Show Name
        nameText.text = curWeapon.WeaponName;

        // Show Ammo
        if (curWeapon as Gun) ammoText.text = $"{(curWeapon as Gun).gunData.loadedBullets} / {(curWeapon as Gun).gunData.magazineSize}";
    }
    #endregion
}
