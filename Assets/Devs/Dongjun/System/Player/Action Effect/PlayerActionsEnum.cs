public enum PlayerActions
{
    // Movement
    Jump,
    Dashing,
    DashStart,
    DashEnd,

    // Status
    Knockbacked,
    KnockbackEnd,
    Stunned,
    StunEnd,

    // Damaged
    Damaged,

    // Health Change
    HealthChanged,
    HealthDamaged,
    HealthHealed,

    // Shield Change
    ShieldChanged,
    ShieldDamaged,
    ShieldGained,

    // Mana Change
    ManaChanged,
    ManaUsed,
    ManaGained,

    // Stamina Change
    StaminaChanged,
    StaminaUsed,
    StaminaGained,

    // Attack
    DamageDealt,
    Kill,

    // Weapon
    WeaponHit,
    WeaponMain,
    WeaponSub,
    WeaponSpecial,

    // Gun
    GunHit,
    GunShoot,
    GunReload,

    // Bow
    BowHit,
    BowShoot,

    // Melee
    MeleeBasicHit,
    MeleeBasicAttack,

    MeleeHeavyHit,
    MeleeHeavyAttack,

    MeleeSlamHit,
    MeleeSlamAttack,

    MeleeDashHit,
    MeleeDashAttack,
}
