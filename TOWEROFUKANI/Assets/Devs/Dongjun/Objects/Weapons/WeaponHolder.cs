using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : MonoBehaviour
{
    #region Var: Inspector
    [SerializeField] private Text nameText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Image[] weaponIcon = new Image[3];
    [SerializeField] private Weapon[] weapons = new Weapon[3];
    #endregion

    #region Var: Weapon Holder
    private int currentSlot = 0;
    private Weapon oldWeapon;
    private Weapon curWeapon;
    #endregion


    #region Method Unity
    private void Awake()
    {
        if (weapons == null)
            return;

        // Init Selected Weapon
        curWeapon = weapons[0];
        curWeapon.SelectWeapon(true);

        // Init Icons
        for (int i = 0; i < weaponIcon.Length; i++)
            weaponIcon[i].sprite = weapons[i].WeaponIcon;
    }
    private void Update()
    {
        if (weapons == null)
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
        currentSlot += scroll == 0 ? 0 : scroll > 0 ? -1 : 1;
        currentSlot = currentSlot == -1 ? 2 : currentSlot == weapons.Length ? 0 : currentSlot;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentSlot = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentSlot = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            currentSlot = 2;
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
        nameText.text = curWeapon.WeaponName;

        // Show Weapon Info
        if (curWeapon is Gun)
        {
            Gun gun = curWeapon as Gun;
            ammoText.text = $"{gun.loadedBullets} / {gun.magazineSize}";
        }
        else
        {
            ammoText.text = string.Empty;
        }
    }
    #endregion
}
