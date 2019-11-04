using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private void Awake()
    {
        heath = new IntStat(100, 0, 100);
    }
    
    private static IntStat heath;
    public static IntStat Heath => heath;

    public static float Stamina { get; set; } = 0;

    public static int DamageReceived;
    public static int HealReceived;

    public static void Damage(int amount)
    {
        DamageReceived = amount;
        ItemEffectManager.Trigger(PlayerActions.Damaged);
        PlayerHitEft.Create(GM.PlayerPos);
        heath.Mod_Flat -= DamageReceived;
    }
    public static void Heal(int amount)
    {
        HealReceived = amount;
        ItemEffectManager.Trigger(PlayerActions.Healed);
        heath.Mod_Flat += HealReceived;
    }
}
