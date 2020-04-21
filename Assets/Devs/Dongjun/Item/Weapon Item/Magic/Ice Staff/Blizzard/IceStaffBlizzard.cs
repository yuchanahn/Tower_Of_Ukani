using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStaffBlizzard : MonoBehaviour
{
    [SerializeField] private CircleCollider2D hitArea;
    [SerializeField] private LayerMask hitLayer;

    private IceStaffItem iceStaffItem;
    private OBB_IceStaff_Data data;

    private PlayerActionEvent onKill;

    public void Init(IceStaffItem iceStaffItem, OBB_IceStaff_Data data)
    {
        this.iceStaffItem = iceStaffItem;
        this.data = data;
        iceStaffItem.Sub_DamageTick.SetActive(true);
        iceStaffItem.Sub_ManaUsageTick.SetActive(true);

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
    public void End()
    {
        iceStaffItem.Sub_DamageTick.Reset();
        iceStaffItem.Sub_DamageTick.SetActive(false);
        iceStaffItem.Sub_ManaUsageTick.Reset();
        iceStaffItem.Sub_ManaUsageTick.SetActive(false);

        PlayerActionEventManager.RemoveEvent(PlayerActions.Kill, onKill);

        Destroy(gameObject);
    }

    private void Update()
    {
        if (!data.IsBlizzardActive)
        {
            End();
            return;
        }

        DealDamage();
        UseMana();
    }

    private void DealDamage()
    {
        if (!iceStaffItem.Sub_DamageTick.IsEnded)
            return;

        iceStaffItem.Sub_DamageTick.Reset();

        var hits = Physics2D.OverlapCircleAll(transform.position, hitArea.radius, hitLayer);
        if (hits.Length != 0)
        {
            AttackData attackData = iceStaffItem.Sub_DamagePerTick;
            attackData.damageDealer = gameObject;

            foreach (var hit in hits)
            {
                PlayerStats.Inst.DealDamage(attackData, hit.gameObject,
                    PlayerActions.WeaponHit);
            }
        }
    }
    private void UseMana()
    {
        if (!iceStaffItem.Sub_ManaUsageTick.IsEnded)
            return;

        iceStaffItem.Sub_ManaUsageTick.Reset();

        if (!PlayerStats.Inst.UseMana(iceStaffItem.Sub_ManaUsagePerTick.Value))
            data.IsBlizzardActive = false;
    }
}
