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

    #region Var: Overlap Data
    private OverlapCheckData overlapCheckData;
    #endregion

    #region Var: Player Action Event
    private PlayerActionEvent onDashing;
    private PlayerActionEvent onDashEnd;
    #endregion

    #region Var: Stat
    private AttackData flameDashAttackData;
    #endregion

    #region Var: Effect
    private ParticleSystem flameParticle;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        // Overlap Data
        overlapCheckData = new OverlapCheckData(
            onEnter: overlap =>
            {
                // Deal Damage
                PlayerStats.Inst.DealDamage(flameDashAttackData, overlap.gameObject);
            });

        // Player Action Event
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
    #endregion

    #region Method: Stats
    public override void InitStats()
    {
        flameDashAttackData = new AttackData(5);
    }
    #endregion

    #region Method: Item
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
