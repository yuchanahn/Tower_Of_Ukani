using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBolt : WeaponProjectile
{
    private PlayerActionEvent onKill;

    private void Start()
    {
        onKill = this.NewPlayerActionEvent(() =>
        {
            if (PlayerStats.Inst.CurAttackData.HasValue && PlayerStats.Inst.CurAttackData.Value.damageDealer == gameObject)
            {
                var dropTable = PlayerStats.Inst.KilledMob.GetComponent<Mob_DropItem>();
                if (dropTable == null)
                    return;

                dropTable.add_drop_table(ItemDB.Inst.GetItemPrefab<FrozenSoul>(), 100f, new RangeInt() { min = 1, max = 1 });
            }
        });

        PlayerActionEventManager.AddEvent(PlayerActions.Kill, onKill);
    }
}
