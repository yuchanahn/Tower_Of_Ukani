using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    private void Start()
    {
        Init_HealthBar();
        PlayerStats.AddEvent_OnHealthChange(gameObject, Update_HealthBar);
    }

    private void Init_HealthBar()
    {
        healthBar.value = (float)PlayerStats.Health.Value / PlayerStats.Health.Max;
    }
    private void Update_HealthBar(IntStat health)
    {
        healthBar.value = (float)health.Value / health.Max;
    }
}
