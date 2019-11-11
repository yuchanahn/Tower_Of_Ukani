using System;
using System.Collections.Generic;
using UnityEngine;
using Dongjun.Helper;

public sealed class Dummy_Action : CLA_Action_Base { }

public abstract class CLA_Main : MonoBehaviour
{
    #region Var: Inspector
    [SerializeField] private CLA_Action_Base defaultAction;
    #endregion

    #region Var: Condition Logics
    protected enum When
    {
        AnyAction,
        OnEnable,
        OnDisable,
    }
    private Dictionary<When, Func<CLA_Action_Base>> logics_Event
        = new Dictionary<When, Func<CLA_Action_Base>>();

    private Dictionary<CLA_Action_Base, Func<CLA_Action_Base>> logics_Action
        = new Dictionary<CLA_Action_Base, Func<CLA_Action_Base>>();
    #endregion

    #region Var: Properties
    protected CLA_Action_Base DefaultAction => defaultAction;
    protected CLA_Action_Base CurrentAction { get; private set; } = null;
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        if (defaultAction == null)
            Debug.LogError("CLA Main Must Have a Default Action!");

        Init();
    }
    protected virtual void OnEnable()
    {
        // Run Action Logic
        if (CurrentAction != null)
        {
            if (logics_Event.ContainsKey(When.OnEnable))
                ChangeAction(logics_Event[When.OnEnable]());

            CurrentAction.OnEnter();
        }

        // Set Current Action to Default Action
        if (CurrentAction == null)
            CurrentAction = defaultAction;
    }
    protected virtual void OnDisable()
    {
        if (logics_Event.ContainsKey(When.OnDisable))
            ChangeAction(logics_Event[When.OnDisable]());
    }
    private void Update()
    {
        // Run Action Logic
        CurrentAction?.OnUpdate();
    }
    private void LateUpdate()
    {
        // Run Action Logic
        if (CurrentAction.CanExecute_OnLateEnter)
        {
            CurrentAction?.OnLateEnter();
            CurrentAction.CanExecute_OnLateEnter = false;
        }
        CurrentAction?.OnLateUpdate();

        // Run Condition Logic
        RunConditionLogic();
    }
    private void FixedUpdate()
    {
        // Run Action Logic
        CurrentAction?.OnFixedUpdate();
    }
    #endregion

    #region Method: Init
    protected abstract void Init();
    #endregion

    #region Method: Change Action
    protected void ChangeAction(CLA_Action_Base action)
    {
        if (action == null || action == CurrentAction)
            return;

        CurrentAction?.OnExit();
        CurrentAction = action;
        CurrentAction?.OnEnter();
        CurrentAction.CanExecute_OnLateEnter = true;
    }
    protected virtual void RunConditionLogic()
    {
        if (logics_Action.ContainsKey(CurrentAction))
            ChangeAction(logics_Action[CurrentAction]());

        if (logics_Event.ContainsKey(When.AnyAction))
            ChangeAction(logics_Event[When.AnyAction]());
    }
    #endregion

    #region Method: Condition Logic
    protected void AddLogic(When when, Func<CLA_Action_Base> logic)
    {
        logics_Event.Add(when, logic);
    }
    protected void AddLogic<T>(ref T when, Func<CLA_Action_Base> logic)
        where T : CLA_Action_Base
    {
        when = GetComponent<T>();
        logics_Action.Add(when, logic);
    }
    #endregion
}
