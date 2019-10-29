using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : MonoBehaviour
{
    public static WeaponHolder Inst { get; private set; }

    #region Var: Inspector
    [SerializeField] private Text nameText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Sprite emptyImage;
    [SerializeField] private Image[] weaponIcon = new Image[3];
    [SerializeField] private WeaponItem[] weapons = new WeaponItem[3];
    #endregion

    #region Var: Weapon Holder
    private int currentSlot = 0;
    private WeaponItem oldWeapon;
    private WeaponItem curWeapon;
    #endregion

    #region Var: Properites
    public WeaponItem currentWeapon => weapons[currentSlot];
    #endregion

    #region Method: Unity
    private void Awake()
    {
        Inst = this;
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

    #region Method: Add / Remove
    public bool HotbarAvailable()
    {
        return weapons[weapons.Length - 1] is null;
    }
    public void AddWeapon(WeaponItem weapon)
    {
        if (!HotbarAvailable())
            return;

        if (weapons[currentSlot] is null)
        {
            weapons[currentSlot] = weapon;
            weaponIcon[currentSlot].sprite = weapons[currentSlot].Info.Icon;
            weapons[currentSlot].SelectWeapon(true);
            return;
        }

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] is null)
            {
                weapons[i] = weapon;
                weaponIcon[i].sprite = weapons[i].Info.Icon;

                if (currentSlot == i)
                    weapons[i].SelectWeapon(true);

                break;
            }
        }
    }
    public void AddWeapon(WeaponItem weapon, int index)
    {
        if (index < 0 || index > weapons.Length - 1)
            return;

        if (!HotbarAvailable())
            return;

        weapons[index] = weapon;
        weaponIcon[index].sprite = weapons[index].Info.Icon;

        if (currentSlot == index)
            weapons[index].SelectWeapon(true);
    }
    
    public void RemoveWeapon(int index)
    {
        if (index < 0 || index > weapons.Length - 1)
            return;

        weapons[index] = null;
        weaponIcon[index].sprite = emptyImage;
    }
    public void RemoveWeapon()
    {
        if (weapons[currentSlot] is null)
            return;

        weapons[currentSlot] = null;
        weaponIcon[currentSlot].sprite = emptyImage;
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
        curWeapon?.SelectWeapon(true);
        oldWeapon = curWeapon;
    }
    #endregion

    #region Method: UI
    private void UpdateUI()
    {
        nameText.text = string.Empty;
        ammoText.text = string.Empty;

        if (curWeapon is null)
            return;

        // Show Name
        nameText.text = curWeapon.Info.Name;

        // Show Weapon Info
        if (curWeapon is GunItem)
        {
            GunItem gun = curWeapon as GunItem;
            ammoText.text = $"{gun.loadedBullets} / {gun.magazineSize.Value}";
        }
    }
    #endregion
}
