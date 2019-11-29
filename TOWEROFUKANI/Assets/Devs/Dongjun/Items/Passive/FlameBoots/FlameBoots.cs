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
    private ItemEffect onDashingEffect;
    private ItemEffect onDashEndEffect;
    #endregion

    #region Var: Collision Detection
    private List<Collider2D> prevHits = new List<Collider2D>();
    #endregion

    #region Var: Flame Dash
    private AttackData flameDashDamage;
    #endregion

    #region Var: Effects
    private ParticleSystem flameParticle;
    #endregion

    #region Method: Unity
    private void Start()
    {
        flameDashDamage = new AttackData(5);
    }
    #endregion

    #region Method Override: Add/Remove
    public override void OnAdd()
    {
        onDashingEffect = new ItemEffect(GetType(), FlameDash);
        onDashEndEffect = new ItemEffect(GetType(), ResetFlameDash);
        ItemEffectManager.AddEffect(PlayerActions.Dashing, onDashingEffect);
        ItemEffectManager.AddEffect(PlayerActions.DashEnd, onDashEndEffect);

        // Spawn Effect
        flameParticle = Instantiate(flameParticlePrefab, GM.PlayerObj.transform).GetComponent<ParticleSystem>();
        flameParticle.Stop();
    }
    public override void OnRemove()
    {
        base.OnRemove();
        ItemEffectManager.RemoveEffect(PlayerActions.Dashing, onDashingEffect);
        ItemEffectManager.RemoveEffect(PlayerActions.DashEnd, onDashEndEffect);

        // Destory Effect
        Destroy(flameParticle.gameObject);
    }
    #endregion

    #region Method Override: Set Bonus Stats
    protected override void SetBonusStats(WeaponItem weapon)
    {

    }
    #endregion

    #region Method: Item Effect
    private void FlameDash()
    {
        // Get Overlaps
        Collider2D[] hits = Physics2D.OverlapBoxAll(GM.PlayerPos, damageSize, 0f, damageLayer);

        if (hits.Length == 0)
        {
            prevHits.Clear();
            return;
        }

        // Check Exit
        for (int i = prevHits.Count - 1; i >= 0; i--)
        {
            if (Array.Exists(hits, col => col == prevHits[i]))
                continue;

            prevHits.RemoveAt(i);
        }

        // Check Enter
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Player") || prevHits.Contains(hits[i]))
                continue;

            prevHits.Add(hits[i]);

            // Deal Damage
            PlayerStats.DealDamage(hits[i].GetComponent<IDamage>(), flameDashDamage);
        }

        // Show Effect
        flameParticle.Play();
    }
    private void ResetFlameDash()
    {
        prevHits.Clear();

        // Hide Effect
        flameParticle.Stop();
    }
    #endregion
}
