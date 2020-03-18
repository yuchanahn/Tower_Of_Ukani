using System.Collections.Generic;
using UnityEngine;

public class Bloodseeker : ActiveItem
{
    #region Var: Inspector
    [SerializeField] private GameObject shieldEffectPrefab;
    #endregion

    #region Var: Stats
    private TimerData durationTimer = new TimerData();
    private FloatStat shieldhealth;
    #endregion

    #region Var: Effect
    private GameObject shieldEffect;
    #endregion

    #region Var: Item Effect
    private PlayerActionEvent onKill;
    List<GameObject> coprsePrefabs = new List<GameObject>();

    private PlayerActionEvent onDamageReceived;
    #endregion

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
                if (!IsActive)
                    return;

                coprsePrefabs.Add(coprpsePrefab);
                shieldhealth.ModFlat += 5;

                // Show Effect
                shieldEffect.SetActive(true);
            });
        });

        onDamageReceived = this.NewPlayerActionEvent(() =>
        {
            // Calculate Overkill Damage
            float overkillDmg = Mathf.Max(PlayerStats.Inst.DamageReceived - shieldhealth.Value, 0);

            // Damage Shield
            shieldhealth.ModFlat -= PlayerStats.Inst.DamageReceived;

            // Damage Player
            PlayerStats.Inst.DamageReceived = overkillDmg;

            // Hide Effect
            if (shieldhealth.Value == 0)
                shieldEffect.SetActive(false);
        });
    }
    #endregion

    #region Method: Stats
    public override void InitStats()
    {
        // Init Cooldown
        cooldownTimer.EndTime = 20f;

        // Init Duration
        durationTimer.EndTime = 5f;
        durationTimer
            .SetTick(gameObject)
            .SetAction(onEnd: () => 
            {
                PlayerStats.Inst.Heal(shieldhealth.Value);
                Deactivate();
            })
            .SetActive(false);

        // Init Shield HP
        shieldhealth = new FloatStat(0, min: 0);
    }
    #endregion

    #region Method: Item
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        // Spawn Effect
        shieldEffect = Instantiate(shieldEffectPrefab, GM.Player.transform.GetChild(0));
        shieldEffect.SetActive(false);
    }
    protected override void OnRemovedFromInventory()
    {
        base.OnRemovedFromInventory();

        // Destroy Effect
        Destroy(shieldEffect);
    }
    #endregion

    #region Method: Activate / Deactivate
    protected override void OnActivate()
    {
        // Stop Cooldown Timer
        cooldownTimer.SetActive(false);
        cooldownTimer.Reset();

        // Start Duration Timer
        durationTimer.SetActive(true);
        durationTimer.Reset();

        PlayerActionEventManager.AddEvent(PlayerActions.Kill, onKill);
        PlayerActionEventManager.AddEvent(PlayerActions.HealthDamaged, onDamageReceived);
    }
    public override void Deactivate()
    {
        base.Deactivate();

        // Start Cooldown Timer
        cooldownTimer.SetActive(true);
        cooldownTimer.Reset();

        // Stop Duration Timer
        durationTimer.SetActive(false);
        durationTimer.Reset();

        // Reset Shield HP
        shieldhealth.Reset();

        // Clear Coprse Prefabs
        coprsePrefabs.Clear();

        // Hide Effect
        shieldEffect.SetActive(false);

        PlayerActionEventManager.RemoveEvent(PlayerActions.Kill, onKill);
        PlayerActionEventManager.RemoveEvent(PlayerActions.HealthDamaged, onDamageReceived);
    }
    #endregion
}
