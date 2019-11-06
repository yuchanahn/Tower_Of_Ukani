﻿using UnityEngine;

public abstract class CLA_Action_Base : MonoBehaviour
{
    [HideInInspector]
    public bool CanExecute_OnLateEnter = true;

    public virtual void OnEnter() { }
    public virtual void OnLateEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }
    public virtual void OnLateUpdate() { }
    public virtual void OnFixedUpdate() { }
}