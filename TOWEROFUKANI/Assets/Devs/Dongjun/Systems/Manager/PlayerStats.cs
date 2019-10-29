using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Heath { get; private set; }
    public static int Stamina { get; private set; }

    public static int DamageReceived;
    public static int HealReceived;

    public static void Damage(int amount)
    {
        DamageReceived = amount;
        ItemEffectManager.Trigger(PlayerActions.Damaged);
        PlayerHitEft.Create(GM.PlayerPos);
        Heath -= DamageReceived;
    }
    public static void Heal(int amount)
    {
        HealReceived = amount;
        ItemEffectManager.Trigger(PlayerActions.Healed);
        Heath += HealReceived;
    }
}
