using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWeaponChanger : MonoBehaviour
{
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
    }
}
