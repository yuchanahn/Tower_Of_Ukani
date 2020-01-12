using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private UI_Fill healthBar;

    private void Start()
    {
        Init_HealthBar();
        PlayerStats.Inst.AddEvent_OnHealthChange(gameObject, Update_HealthBar);
    }

    private void Init_HealthBar()
    {
        healthBar.Value = (float)PlayerStats.Inst.Health.Value / PlayerStats.Inst.Health.Max;
    }

    private void Update_HealthBar(IntStat health)
    {
        healthBar.Value = (float)health.Value / health.Max;
    }
}
