using UnityEngine;

public struct AttackData
{
    public FloatStat damage;
    // Damage Type
    // Knockback
    // Penetration
    // Etc...

    public AttackData(float damage = 0)
    {
        this.damage = new FloatStat(damage, min: 0);
    }
}
