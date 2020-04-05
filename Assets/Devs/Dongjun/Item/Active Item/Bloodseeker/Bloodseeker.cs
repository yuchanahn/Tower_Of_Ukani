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
    private float shieldGainPerKill = 5;
    private FloatStat shieldHealth;

    float healMultiplier = 0.6f;
    #endregion

    #region Var: Item Effect
    private PlayerActionEvent onKill;
    private PlayerActionEvent onShieldChanged;

    int corpsesToAbsorb = 0;
    List<GameObject> coprsePrefabs = new List<GameObject>();
    Coroutine corutine_CheckAllCorpseAbsorbed;
    #endregion

    #region Var: Visual Effect
    private GameObject shieldEffect;
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
                    corutine_CheckAllCorpseAbsorbed = StartCoroutine(CheckAllCorpseAbsorbed());
                    PlayerActionEventManager.RemoveEvent(PlayerActions.Kill, onKill);
                })
            .SetActive(false);

        IEnumerator CheckAllCorpseAbsorbed()
        {
            while (true)
            {
                // Check if All Corpses all Absorbed
                if (IsActive && durationTimer.IsEnded && corpsesToAbsorb == 0)
                {
                    PlayerStats.Inst.Heal(PlayerStats.Inst.GetShieldAt(shieldIndex).Value * healMultiplier);

                    // TODO
                    // 시체 블록 풀링하기 때문에 소환할때 리셋 해야함!!!
                    //for (int i = 0; i < Mathf.FloorToInt(PlayerStats.Inst.GetShieldAt(shieldIndex).Value / shieldGainPerKill); i++)
                    //{
                    //    coprsePrefabs[i].
                    //        GetComponent<PoolingObj>().Spawn(GM.PlayerPos).
                    //        GetComponent<Rigidbody2D>().AddForce(Vector2.right * GM.Player.Data.Dir * 50f, ForceMode2D.Impulse);
                    //}

                    Deactivate();

                    corutine_CheckAllCorpseAbsorbed = null;
                    yield return null;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        // Init Shield HP
        shieldHealth = new FloatStat(0, min: 0, max: 40);
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

                corpsesToAbsorb--;
                coprsePrefabs.Add(coprpsePrefab);

                // Add Shield
                PlayerStats.Inst.GetShieldAt(shieldIndex).ModFlat += shieldGainPerKill;

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

        if (corutine_CheckAllCorpseAbsorbed != null)
            StopCoroutine(corutine_CheckAllCorpseAbsorbed);

        // Hide Effect
        shieldEffect.SetActive(false);

        PlayerActionEventManager.RemoveEvent(PlayerActions.Kill, onKill);
        PlayerActionEventManager.RemoveEvent(PlayerActions.ShieldChanged, onShieldChanged);
    }
    #endregion
}
