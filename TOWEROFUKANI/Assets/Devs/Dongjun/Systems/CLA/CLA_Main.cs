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
        CurrentAction?.OnChange();
    }
    private void Update()
    {
        CurrentAction?.OnUpdate();
    }
    private void LateUpdate()
    {
        if (CurrentAction.CanExecuteOnStart)
        {
            CurrentAction?.OnStart();
            CurrentAction.CanExecuteOnStart = false;
        }

        CurrentAction?.OnLateUpdate();
        CheckCondition();
    }
    private void FixedUpdate()
    {
        CurrentAction?.OnFixedUpdate();
    }

    protected abstract void Init();
    protected void ChangeAction(CLA_Action action)
    {
        if (action == CurrentAction)
            return;

        CurrentAction?.OnEnd();
        CurrentAction = action;
        CurrentAction?.OnChange();
        CurrentAction.CanExecuteOnStart = true;
    }
    protected void ForceChangeAction(CLA_Action action)
    {
        CurrentAction?.OnEnd();
        CurrentAction = action;
        CurrentAction?.OnChange();
        CurrentAction.CanExecuteOnStart = true;
    }
    private void CheckCondition()
    {
        if (CurrentAction != null && ConditionLogics.ContainsKey(CurrentAction))
            ConditionLogics[CurrentAction]?.Invoke();
    }
}
