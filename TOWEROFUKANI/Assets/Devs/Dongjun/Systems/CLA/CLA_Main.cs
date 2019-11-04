using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CLA_Main : MonoBehaviour
{
    #region Var: Inspector
    [SerializeField] private CLA_Action defaultAction;
    #endregion

    #region Var: Condition Logics
    protected Dictionary<CLA_Action, Func<CLA_Action>> ConditionLogics = new Dictionary<CLA_Action, Func<CLA_Action>>();
    #endregion

    #region Var: Properties
    protected CLA_Action DefaultAction => defaultAction;
    protected CLA_Action CurrentAction { get; private set; }
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        if (defaultAction is null)
            Debug.LogError("It Needs Default Action!");

        Init();
    }
    protected virtual void Start()
    {
        CurrentAction = defaultAction;
        CurrentAction?.OnEnter();
    }
    private void Update()
    {
        CurrentAction?.OnUpdate();
    }
    private void LateUpdate()
    {
        if (CurrentAction.CanExecute_OnLateEnter)
        {
            CurrentAction?.OnLateEnter();
            CurrentAction.CanExecute_OnLateEnter = false;
        }

        CurrentAction?.OnLateUpdate();
        RunConditionLogic();
    }
    private void FixedUpdate()
    {
        CurrentAction?.OnFixedUpdate();
    }
    #endregion

    #region Method: Init
    protected abstract void Init();
    #endregion

    #region Method: Change Action
    private void ChangeAction(CLA_Action action)
    {
        if (action == CurrentAction)
            return;

        CurrentAction?.OnExit();
        CurrentAction = action;
        CurrentAction?.OnEnter();
        CurrentAction.CanExecute_OnLateEnter = true;
    }
    private void RunConditionLogic()
    {
        if (CurrentAction != null && ConditionLogics.ContainsKey(CurrentAction))
            ChangeAction(ConditionLogics[CurrentAction]());
    }
    #endregion
}
