using UnityEngine;

[System.Serializable]
public struct AttackData
{
    public IntStat damage;
    // Damage Type
    // Knockback
    // Penetration
    // Etc...

    public AttackData(int damage)
    {
        this.damage = new IntStat(damage, min: 0);
    }
}
