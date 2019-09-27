using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CLA_Main : MonoBehaviour
{
    [SerializeField]
    private CLA_Action defaultAction;
    protected CLA_Action DefaultAction => defaultAction;
    protected CLA_Action CurrentAction { get; private set; }

    protected Dictionary<CLA_Action, Action> ConditionLogics = new Dictionary<CLA_Action, Action>();


    protected virtual void Awake()
    {
        if (defaultAction == null)
        {
            Debug.LogError("It Needs Default Action!");
        }

        Init();
    }
    protected virtual void Start()
    {
        CurrentAction = defaultAction;
        CurrentAction?.OnStart();
    }
    private void Update()
    {
        CurrentAction?.OnUpdate();
        CheckCondition();
    }
    private void LateUpdate()
    {
        CurrentAction?.OnLateUpdate();
    }
    private void FixedUpdate()
    {
        CurrentAction?.OnFixedUpdate();
    }

    protected abstract void Init();
    protected void ChangeAction(CLA_Action action)
    {
        CurrentAction?.OnEnd();
        CurrentAction = action;
        CurrentAction?.OnStart();
    }
    private void CheckCondition()
    {
        if (CurrentAction != null && ConditionLogics.ContainsKey(CurrentAction))
            ConditionLogics[CurrentAction]?.Invoke();
    }
}
