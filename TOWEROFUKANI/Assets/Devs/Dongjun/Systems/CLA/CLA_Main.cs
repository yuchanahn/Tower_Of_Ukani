using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CLA_Main : MonoBehaviour
{
    #region Var: Inspector
    [SerializeField] private CLA_Action_Base defaultAction;
    #endregion

    #region Var: Condition Logics
    protected Dictionary<CLA_Action_Base, Func<CLA_Action_Base>> ConditionLogics = 
        new Dictionary<CLA_Action_Base, Func<CLA_Action_Base>>();
    #endregion

    #region Var: Properties
    protected CLA_Action_Base DefaultAction => defaultAction;
    protected CLA_Action_Base CurrentAction { get; private set; } = null;
    #endregion

    #region Method: Unity
    protected virtual void OnEnable()
    {
        CurrentAction?.OnEnter();

        if (CurrentAction is null)
            CurrentAction = defaultAction;
    }
    protected virtual void OnDisable()
    {
        CurrentAction?.OnExit();
    }
    protected virtual void Awake()
    {
        if (defaultAction is null)
            Debug.LogError("CLA Main Must Have a Default Action!");

        Init();
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
    private void ChangeAction(CLA_Action_Base action)
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
