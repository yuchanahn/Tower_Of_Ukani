using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStaminaBar : MonoBehaviour
{
    [SerializeField] private Transform staminaBar;
    private Image[] staminaBars = new Image[3];

    private void Awake()
    {
        Init_StaminaBar();
        PlayerStats.AddEvent_OnStaminaChange(gameObject, Update_StaminaBar);
    }

    private void Init_StaminaBar()
    {
        for (int i = 0; i < staminaBars.Length; i++)
        {
            staminaBars[i] = staminaBar.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>();
        }

        for (int i = 0; i < staminaBars.Length; i++)
        {
            staminaBars[i].fillAmount = PlayerStats.Stamina.Value >= i + 1 ? 1 : PlayerStats.Stamina.Value - i;
        }
    }
    private void Update_StaminaBar(FloatStat stamina)
    {
        for (int i = 0; i < staminaBars.Length; i++)
        {
            staminaBars[i].fillAmount = stamina.Value >= i + 1 ? 1 : stamina.Value - i;
        }
    }
}
