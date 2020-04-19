using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStaffBlizzard : MonoBehaviour
{
    [SerializeField] private CircleCollider2D hitArea;
    [SerializeField] private LayerMask hitLayer;

    private IceStaffItem iceStaffItem;
    private OBB_IceStaff_Data data;

    public void Init(IceStaffItem iceStaffItem, OBB_IceStaff_Data data)
    {
        this.iceStaffItem = iceStaffItem;
        this.data = data;
        iceStaffItem.Sub_DamageTick.SetActive(true);
        iceStaffItem.Sub_ManaUsageTick.SetActive(true);
    }
    public void End()
    {
        iceStaffItem.Sub_DamageTick.Reset();
        iceStaffItem.Sub_DamageTick.SetActive(false);
        iceStaffItem.Sub_ManaUsageTick.Reset();
        iceStaffItem.Sub_ManaUsageTick.SetActive(false);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!data.IsBlizzardActive)
        {
            End();
            return;
        }

        if (iceStaffItem.Sub_DamageTick.IsEnded)
        {
            iceStaffItem.Sub_DamageTick.Reset();

            var hits = Physics2D.OverlapCircleAll(transform.position, hitArea.radius, hitLayer);
            if (hits.Length != 0)
            {
                foreach (var hit in hits)
                {
                    PlayerStats.Inst.DealDamage(iceStaffItem.Sub_DamagePerTick, hit.gameObject,
                        PlayerActions.WeaponHit);
                }
            }
        }

        if (iceStaffItem.Sub_ManaUsageTick.IsEnded)
        {
            iceStaffItem.Sub_ManaUsageTick.Reset();

            if (!PlayerStats.Inst.UseMana(iceStaffItem.Sub_ManaUsagePerTick.Value))
                data.IsBlizzardActive = false;
        }
    }
}
