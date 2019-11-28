using System;
using System.Collections.Generic;
using UnityEngine;

public class FlameBoots : PassiveItem
{
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private Vector2 damageSize;
    
    private ItemEffect onDashingEffect;
    private ItemEffect onDashEndEffect;

    private List<Collider2D> prevHits = new List<Collider2D>();
    private AttackData flameDashDamage;

    private void Start()
    {
        flameDashDamage = new AttackData(5);
    }

    public override void OnAdd()
    {
        onDashingEffect = new ItemEffect(GetType(), FlameDash);
        onDashEndEffect = new ItemEffect(GetType(), ResetFlameDash);
        ItemEffectManager.AddEffect(PlayerActions.Dashing, onDashingEffect);
        ItemEffectManager.AddEffect(PlayerActions.DashEnd, onDashEndEffect);
    }
    public override void OnRemove()
    {
        base.OnRemove();
        ItemEffectManager.RemoveEffect(PlayerActions.Dashing, onDashingEffect);
        ItemEffectManager.RemoveEffect(PlayerActions.DashEnd, onDashEndEffect);
    }

    protected override void SetBonusStats(WeaponItem weapon)
    {

    }

    private void FlameDash()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(GM.PlayerPos, damageSize, 0f, damageLayer);

        if (hits.Length == 0)
        {
            prevHits.Clear();
            return;
        }

        for (int i = prevHits.Count - 1; i >= 0; i--)
        {
            if (Array.Exists(hits, col => col == prevHits[i]))
                continue;

            prevHits.RemoveAt(i);
        }

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Player") || prevHits.Contains(hits[i]))
                continue;

            prevHits.Add(hits[i]);

            // Deal Damage
            PlayerStats.DealDamage(hits[i].GetComponent<IDamage>(), flameDashDamage);
        }
    }
    private void ResetFlameDash()
    {
        prevHits.Clear();
    }
}
