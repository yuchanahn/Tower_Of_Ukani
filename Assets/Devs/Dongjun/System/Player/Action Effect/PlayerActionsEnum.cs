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

    // Health Change
    HealthChanged,
    HealthDamaged,
    HealthHealed,

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
