using Dongjun.Helper;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlameBoots : PassiveItem
{
    #region Var: Inspector
    [Header("Damage")]
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private Vector2 damageSize;

    [Header("Effect")]
    [SerializeField] private GameObject flameParticlePrefab;
    #endregion

    #region Var: Item Stats
    private AttackData flameDashAttackData;
    private OverlapCheckData overlapCheckData;
    #endregion

    #region Var: Item Effect
    private PlayerActionEvent onDashing;
    private PlayerActionEvent onDashEnd;
    #endregion

    #region Var: Visual Effect
    private ParticleSystem flameParticle;
    #endregion

    #region Method: Item
    public override void InitStats()
    {
        flameDashAttackData = new AttackData(5);
    }
    protected override void InitEvents()
    {
        overlapCheckData = new OverlapCheckData(
            onEnter: overlap =>
            {
                // Deal Damage
                PlayerStats.Inst.DealDamage(flameDashAttackData, overlap.gameObject);
            });

        onDashing = this.NewPlayerActionEvent(() =>
        {
            // Check Overlap
            overlapCheckData.OverlapCheckOnce(Physics2D.OverlapBoxAll(GM.PlayerPos, damageSize, 0f, damageLayer));

            // Play Particle Effect
            flameParticle.Play();
        });

        onDashEnd = this.NewPlayerActionEvent(() =>
        {
            // Clear Overlap Data
            overlapCheckData.Clear();

            // Stop Particle Effect
            flameParticle.Stop();
        });
    }

    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        // Add Player Action Event
        PlayerActionEventManager.AddEvent(PlayerActions.Dashing, onDashing);
        PlayerActionEventManager.AddEvent(PlayerActions.DashEnd, onDashEnd);

        // Spawn Particle
        flameParticle = Instantiate(flameParticlePrefab, GM.PlayerObj.transform).GetComponent<ParticleSystem>();
        flameParticle.transform.localPosition = flameParticle.transform.localPosition.Add(y: -0.2f);
    }
    protected override void OnRemovedFromInventory()
    {
        // Remove Player Action Event
        PlayerActionEventManager.RemoveEvent(PlayerActions.Dashing, onDashing);
        PlayerActionEventManager.RemoveEvent(PlayerActions.DashEnd, onDashEnd);

        // Destory Particle
        Destroy(flameParticle.gameObject);
    }
    #endregion
}
