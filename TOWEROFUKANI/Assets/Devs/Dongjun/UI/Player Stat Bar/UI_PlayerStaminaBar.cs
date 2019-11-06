using UnityEngine;

public class UI_PlayerStaminaBar : MonoBehaviour
{
    [SerializeField] private Transform staminaBarParent;
    [SerializeField] private UI_Bar staminaBarPrefab;

    private UI_Bar[] staminaBars;

    private void Awake()
    {
        Init_StaminaBar();
        PlayerStats.AddEvent_OnStaminaChange(gameObject, Update_StaminaBar);
    }

    private void Init_StaminaBar()
    {
        staminaBars = new UI_Bar[(int)PlayerStats.Stamina.Max];

        for (int i = 0; i < staminaBars.Length; i++)
        {
            staminaBars[i] = Instantiate(staminaBarPrefab, staminaBarParent).GetComponent<UI_Bar>();
        }

        for (int i = 0; i < staminaBars.Length; i++)
        {
            staminaBars[i].Value = PlayerStats.Stamina.Value >= i + 1 ? 1 : PlayerStats.Stamina.Value - i;
        }
    }
    private void Update_StaminaBar(float staminaBarValue)
    {
        for (int i = 0; i < staminaBars.Length; i++)
        {
            staminaBars[i].Value = staminaBarValue >= i + 1 ? 1 : staminaBarValue - i;
        }
    }
}
