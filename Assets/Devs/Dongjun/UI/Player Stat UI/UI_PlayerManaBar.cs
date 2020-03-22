using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerManaBar : MonoBehaviour
{
    [SerializeField] private UI_Fill manaBar;

    private void Start()
    {
        Update_ManaBar();
        PlayerActionEventManager.AddEvent(PlayerActions.ManaChanged, this.NewPlayerActionEvent(Update_ManaBar));
    }

    private void Update_ManaBar()
    {
        manaBar.Value = PlayerStats.Inst.mana.Value / PlayerStats.Inst.mana.Max;
    }
}
