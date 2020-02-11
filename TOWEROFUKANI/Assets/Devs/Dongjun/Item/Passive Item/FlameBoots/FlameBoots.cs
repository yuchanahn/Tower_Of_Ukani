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

    #region Var: Item Effect
    private ActionEffect onDashingEffect;
    private ActionEffect onDashEndEffect;
    #endregion

    #region Var: Collision Detection
    private OverlapCheckData overlapCheckData;
    #endregion

    #region Var: Flame Dash
    private AttackData flameDashAttackData;
    #endregion

    #region Var: Effects
    private ParticleSystem flameParticle;
    #endregion

    #region Method: Stats
    public override void InitStats()
    {
        flameDashAttackData = new AttackData(5);
    }
    #endregion

    #region Method Override: Add/Remove
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        // Init Data
        overlapCheckData = new OverlapCheckData(onEnter: OnFlameDashHit);

        onDashingEffect = this.CreateItemEffect(FlameDash);
        onDashEndEffect = this.CreateItemEffect(ResetFlameDash);

        ActionEffectManager.AddEffect(PlayerActions.Dashing, onDashingEffect);
        ActionEffectManager.AddEffect(PlayerActions.DashEnd, onDashEndEffect);

        // Spawn Effect
        flameParticle = Instantiate(flameParticlePrefab, GM.PlayerObj.transform).GetComponent<ParticleSystem>();
        flameParticle.transform.localPosition = flameParticle.transform.localPosition.Add(y: -0.2f);
    }
    public override void OnDrop()
    {
        base.OnDrop();

        // Remove Item Effect
        ActionEffectManager.RemoveEffect(PlayerActions.Dashing, onDashingEffect);
        ActionEffectManager.RemoveEffect(PlayerActions.DashEnd, onDashEndEffect);

        // Destory Effect
        Destroy(flameParticle.gameObject);
    }
    #endregion

    #region Method: Item Effect
    private void FlameDash()
    {
        // Get Overlaps
        overlapCheckData.OverlapCheck(Physics2D.OverlapBoxAll(GM.PlayerPos, damageSize, 0f, damageLayer));

        // Show Effect
        flameParticle.Play();
    }
    private void OnFlameDashHit(Collider2D overlap)
    {
        PlayerStats.Inst.DealDamage(flameDashAttackData, overlap.gameObject);
    }

    private void ResetFlameDash()
    {
        // Clear Overlaps
        overlapCheckData.Clear();

        // Hide Effect
        flameParticle.Stop();
    }
    #endregion
}
