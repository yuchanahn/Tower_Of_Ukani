using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : PassiveItem
{
    ItemEffect onHitEffect = new ItemEffect();

    public override void OnAdd()
    {
        ItemEffectManager.AddEffect(PlayerActions.Hit, onHitEffect);
    }

    protected override void SetBonusStats(Item item)
    {

    }
}
