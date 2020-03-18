using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private UI_Fill healthBar;

    private void Start()
    {
        Update_HealthBar();
        PlayerActionEventManager.AddEvent(PlayerActions.HealthChanged, this.NewPlayerActionEvent(Update_HealthBar));
    }

    private void Update_HealthBar()
    {
        healthBar.Value = (float)PlayerStats.Inst.health.Value / PlayerStats.Inst.health.Max;
    }
}
