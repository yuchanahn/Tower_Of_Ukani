using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempWeaponChanger : MonoBehaviour
{
    [SerializeField] private Text ammoText;
    [SerializeField] private Weapon[] weaponSlot = new Weapon[3];

    private Weapon curWeapon;

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

        if (Input.GetKeyDown(KeyCode.Alpha1) && curWeapon != weaponSlot[0])
        {
            curWeapon.SelectWeapon(false);
            curWeapon = weaponSlot[0];
            curWeapon.SelectWeapon(true);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && curWeapon != weaponSlot[1])
        {
            curWeapon.SelectWeapon(false);
            curWeapon = weaponSlot[1];
            curWeapon.SelectWeapon(true);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && curWeapon != weaponSlot[2])
        {
            curWeapon.SelectWeapon(false);
            curWeapon = weaponSlot[2];
            curWeapon.SelectWeapon(true);
            return;
        }

        ammoText.text = $"{(curWeapon as Gun).gunData.loadedBullets} / {(curWeapon as Gun).gunData.magazineSize}";
    }
}
