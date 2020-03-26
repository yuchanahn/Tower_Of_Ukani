using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Bloodseeker : ActiveItem
{
    #region Var: Inspector
    [SerializeField] private GameObject shieldEffectPrefab;
    #endregion

    #region Var: Item Stats
    private TimerData durationTimer = new TimerData();

    private int shieldIndex = -1;
    private FloatStat shieldHealth;
    #endregion

    #region Var: Item Effect
    private PlayerActionEvent onKill;
    private PlayerActionEvent onShieldChanged;

    int corpsesToAbsorb = 0;
    List<GameObject> coprsePrefabs = new List<GameObject>();
    Coroutine checkAllCorpseAbsorbed;
    #endregion

    #region Var: Visual Effect
    private GameObject shieldEffect;
    #endregion

    #region Method: Unity
    private IEnumerator CheckAllCorpseAbsorbed()
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();

            if (IsActive && durationTimer.IsEnded && corpsesToAbsorb == 0)
            {
                PlayerStats.Inst.Heal(shieldHealth.Value * 0.6f);
                Deactivate();

                checkAllCorpseAbsorbed = null;
                yield return null;
            }
        }
    }
    #endregion

    #region Method: Item
    public override void InitStats()
    {
        // Init Cooldown
        CooldownTimer.EndTime = 10f;

        // Init Mana Usage
        ManaUsage = new FloatStat(20, min: 0);

        // Init Duration
        durationTimer.EndTime = 10f;
        durationTimer
            .SetTick(gameObject)
            .SetAction(
                onEnd: () =>
                {
                    checkAllCorpseAbsorbed = StartCoroutine(CheckAllCorpseAbsorbed());
                    PlayerActionEventManager.RemoveEvent(PlayerActions.Kill, onKill);
                })
            .SetActive(false);

        // Init Shield HP
        shieldHealth = new FloatStat(0, min: 0, max: 20);
    }
    protected override void InitEvents()
    {
        onKill = this.NewPlayerActionEvent(() =>
        {
            // Check Corpse Spawner
            var corpseSpawner = PlayerStats.Inst.KilledMob.GetComponent<CorpseSpawner>();
            if (corpseSpawner == null)
                return;

            // Add Corpse Count
            corpsesToAbsorb += corpseSpawner.CorpseCount;

            // On Corpse Absorb
            corpseSpawner.SetCorpseMode(eCorpseSpawnMode.Absorb, coprpsePrefab =>
            {
                if (!IsActive)
                    return;

                coprsePrefabs.Add(coprpsePrefab);
                corpsesToAbsorb -= 1;

                // Add Shield
                PlayerStats.Inst.GetShieldAt(shieldIndex).ModFlat += 5;

                // Show Visual Effect
                shieldEffect.SetActive(true);
            });
        });

        onShieldChanged = this.NewPlayerActionEvent(() =>
        {
            // Check Shield Health
            if (PlayerStats.Inst.GetShieldAt(shieldIndex).IsMin)
            {
                // Hide Visual Effect
                shieldEffect.SetActive(false);
            }
        });
    }

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

    protected override void OnActivate()
    {
        // Stop Cooldown Timer
        CooldownTimer.SetActive(false);
        CooldownTimer.Reset();

        // Start Duration Timer
        durationTimer.SetActive(true);
        durationTimer.Reset();

        // Get Shield Index
        shieldIndex = PlayerStats.Inst.AddShield(shieldHealth);

        PlayerActionEventManager.AddEvent(PlayerActions.Kill, onKill);
        PlayerActionEventManager.AddEvent(PlayerActions.ShieldChanged, onShieldChanged);
    }
    protected override void OnDeactivate()
    {
        // Stop Duration Timer
        durationTimer.SetActive(false);
        durationTimer.Reset();

        // Remove Shield
        PlayerStats.Inst.RemoveShield(ref shieldIndex);

        corpsesToAbsorb = 0;
        coprsePrefabs.Clear();

        if (checkAllCorpseAbsorbed != null)
            StopCoroutine(checkAllCorpseAbsorbed);

        // Hide Effect
        shieldEffect.SetActive(false);

        PlayerActionEventManager.RemoveEvent(PlayerActions.Kill, onKill);
        PlayerActionEventManager.RemoveEvent(PlayerActions.ShieldChanged, onShieldChanged);
    }
    #endregion
}
