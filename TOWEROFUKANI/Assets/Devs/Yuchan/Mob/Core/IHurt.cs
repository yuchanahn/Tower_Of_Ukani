using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHurt
{
    void OnHurt();
    void OnDead(bool absorbCorpses, Action<Corpse> onAbsorb);
}
