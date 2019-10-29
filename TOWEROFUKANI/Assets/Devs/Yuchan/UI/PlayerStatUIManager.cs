using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUIManager : MonoBehaviour
{
    [SerializeField] Slider     HpBar;
    [SerializeField] Slider[]   StaminaBar;
    [SerializeField] float Stamina_BarMax;
    [SerializeField] float StaminaRegen;

    public static int idx = 0;

    static PlayerStatUIManager Inst;


    private void Awake()
    {
        Inst = this;
    }

    public static bool UseStamina()
    {
        if (idx == 0) return false;
        if(idx != Inst.StaminaBar.Length) Inst.StaminaBar[idx].value = 0;
        Inst.StaminaBar[--idx].value = PlayerStats.Stamina / Inst.Stamina_BarMax;
        return true;

    }

    private void Update()
    {
        HpBar.value = (float)PlayerStats.Heath.Value / PlayerStats.Heath.Max;

        // max 면 리턴....
        if (StaminaBar.Length == idx) return;

        PlayerStats.Stamina += StaminaRegen * Time.deltaTime;
        if(PlayerStats.Stamina > Stamina_BarMax)
        {
            PlayerStats.Stamina = 0;
            StaminaBar[idx].value = 1;
            idx++;
        }
        else
        {
            StaminaBar[idx].value = PlayerStats.Stamina / Stamina_BarMax;
        }
    }
}
