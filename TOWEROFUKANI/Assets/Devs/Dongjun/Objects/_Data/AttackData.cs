using UnityEngine;

[System.Serializable]
public struct AttackData
{
    private int damage;
    // Damage Type
    // Knockback
    // Penetration
    // Etc...

    public int Damage
    {
        get { return damage; }
        set { damage = Mathf.Max(value, 0); }
    }

    public AttackData(int damage)
    {
        this.damage = Mathf.Max(damage, 0);
    }
}
