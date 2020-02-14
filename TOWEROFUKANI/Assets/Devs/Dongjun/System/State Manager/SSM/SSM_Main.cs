using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SSM_Main : MonoBehaviour
{
    #region Var: State Change Logics
    protected enum When
    {
        AnyAction,
        OnEnable,
        OnDisable
    }
    private Dictionary<When, Func<SSM_State>> logics_Event = new Dictionary<When, Func<SSM_State>>();
    private Dictionary<SSM_State, Func<SSM_State>> logics_State = new Dictionary<SSM_State, Func<SSM_State>>();
    #endregion

    #region Var: Run Logic
    private bool canRunOnLateEnter = true;
    #endregion

    #region Prop: 
    protected SSM_State DefaultState
    { get; private set; }
    protected SSM_State CurrentState
    { get; private set; }
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        InitStates();

        if (DefaultState == null)
            Debug.LogError("CLA Main Must Have a Default Action!");
    }
    protected virtual void OnEnable()
    {
        // Run Action Logic
        if (CurrentState != null)
        {
            if (logics_Event.ContainsKey(When.OnEnable))
                ChangeState(logics_Event[When.OnEnable]());

            CurrentState.OnEnter();
        }

        // Set Current Action to Default Action
        if (CurrentState == null)
            CurrentState = DefaultState;
    }
    protected virtual void OnDisable()
    {
        if (logics_Event.ContainsKey(When.OnDisable))
            ChangeState(logics_Event[When.OnDisable]());
    }
    private void Update()
    {
        // Run Condition Logic
        RunConditionLogic();

        // Run Action Logic
        CurrentState.OnUpdate();
    }
    private void LateUpdate()
    {
        // Run Action Logic
        if (canRunOnLateEnter)
        {
            CurrentState.OnLateEnter();
            canRunOnLateEnter = false;
        }

        CurrentState.OnLateUpdate();
    }
    private void FixedUpdate()
    {
        // Run Action Logic
        CurrentState?.OnFixedUpdate();
    }
    #endregion

    #region Method: Init
    protected abstract void InitStates();
    protected void SetDefaultState(SSM_State defaultState)
    {
        DefaultState = defaultState;
    }
    #endregion

    #region Method: Change Action
    protected void ChangeState(SSM_State action)
    {
        if (action == null || action == CurrentState)
            return;

        CurrentState?.OnExit();
        CurrentState = action;
        CurrentState?.OnEnter();
        canRunOnLateEnter = true;
    }
    protected virtual void RunConditionLogic()
    {
        if (logics_Event.ContainsKey(When.AnyAction) && logics_Event[When.AnyAction]() != null)
        {
            ChangeState(logics_Event[When.AnyAction]());
        }
        else if(logics_State.ContainsKey(CurrentState))
        {
            ChangeState(logics_State[CurrentState]());
        }
    }
    #endregion

    #region Method: Condition Logic
    protected void SetLogic(When when_Event, Func<SSM_State> logic)
    {
        logics_Event.Add(when_Event, logic);
    }
    protected void SetLogic<T>(ref T when_State, Func<SSM_State> logic)
        where T : SSM_State
    {
        when_State = GetComponent<T>();
        logics_State.Add(when_State, logic);
    }
    #endregion
}
