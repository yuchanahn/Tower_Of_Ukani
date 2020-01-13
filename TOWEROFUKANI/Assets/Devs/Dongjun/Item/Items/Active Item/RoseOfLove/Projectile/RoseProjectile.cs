using UnityEngine;

public class RoseProjectile : WeaponProjectile
{
    protected override void OnHit(Vector2 hitPos, GameObject hitObject)
    {
        base.OnHit(hitPos, hitObject);

        GameObject stunnedEffect = hitObject.GetComponent<StatusEffect_Object>()?.StunnedObj;
        if (stunnedEffect != null)
            StatusEffect_Stunned.Create(hitObject, stunnedEffect, 3f);
    }
}
