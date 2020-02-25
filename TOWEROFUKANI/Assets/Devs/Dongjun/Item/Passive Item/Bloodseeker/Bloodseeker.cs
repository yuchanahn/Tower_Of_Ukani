using System.Collections.Generic;
using UnityEngine;

public class Bloodseeker : PassiveItem
{
    #region Var: Player Action Event
    private PlayerActionEvent onKill;
    #endregion

    List<GameObject> coprsePrefabs = new List<GameObject>();

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        // Player Action Event
        onKill = this.NewPlayerActionEvent(() =>
        {
            // Heal
            PlayerStats.Inst.KilledMob.GetComponent<CorpseSpawner>()?.SetCorpseMode(eCorpseSpawnMode.Absorb, coprpsePrefab => 
                {
                    coprsePrefabs.Add(coprpsePrefab);
                    PlayerStats.Inst.Heal(5);
                });
        });
    }
    #endregion

    #region Method: Item
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        PlayerActionEventManager.AddEvent(PlayerActions.Kill, onKill);
    }
    protected override void OnRemovedFromInventory()
    {
        PlayerActionEventManager.RemoveEvent(PlayerActions.Kill, onKill);
    }
    #endregion
}
