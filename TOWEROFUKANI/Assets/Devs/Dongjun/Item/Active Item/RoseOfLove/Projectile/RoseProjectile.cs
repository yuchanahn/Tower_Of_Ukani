using UnityEngine;

public class RoseProjectile : WeaponProjectile
{
    protected override bool DamageCreature(GameObject hit)
    {
        if (base.DamageCreature(hit))
        {
            GameObject stunnedEffect = hit.GetComponent<StatusEffect_Object>()?.StunnedObj;
            if (stunnedEffect != null)
                StatusEffect_Stunned.Create(hit, stunnedEffect, 3f);

            return true;
        }

        return false;
    }
}
