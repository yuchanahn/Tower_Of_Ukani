using System;
using System.Collections.Generic;
using UnityEngine;

/* OBB: Objective Based Behaviour
 * -> 목표 기반 행동
 * 
 * ------------------------------------------------------------------------
 * 
 * Objective (목표)
 * -> 궁극적으로 원하는 것.
 * 
 * Behaviour (행동)
 * -> State의 묶음.
 * 
 * ------------------------------------------------------------------------
 * 
 * 지원 되는 기능:
 * 
 * -> Objective: ForceRun
 *    조건이 맞다면 무조건(Behaviour: Wait 과 Objective: Transition 무시) 이 목표를 실행함.
 *    
 * -> Objective: Transition
 *    현재 Objective가 끝나면 Transition Objective를 실행함.
 *    (Transition이 끝날 때 까지 다른 Objective는 실행 되지 않음.)
 *    
 * -> Behaviour: Wait
 *    Behaviour가 끝날때까지 다른 상태가 될 수 없음.
 * 
 * ------------------------------------------------------------------------
 * 
 * 초기화:
 * 
 * -> States 초기화
 *    1) State를 가져올 때 GetComponent 대신 GetState(ref State) 함수를 사용해야함.
 *    2) SetDefaultState(State, StateAction) 함수는 무조건 실행해야 함.
 * 
 * -> Behaviour 초기화
 *    1) Single: 하나의 State만 필요할 때 사용함.
 *    2) Process: 두개 이상의 State들이 순차적으로 모두 완수해야 할 때 사용함.
 *    3) Sequence: 두개 이상의 State들을 순차적으로 완수해야 할 때 사용함.
 *    3) Continue: 두개 이상의 State들을 순차적으로 완수해야 하며 다른 행동으로 바뀌었다가 돌아와도 이어서 진행 해야할 때 사용함.
 *    4) Choice: 두개 이상의 State들이 조건에 의해 다른 State로 바뀌어야 할 때 사용함.
 * 
 * -> Objective 초기화
 *    1) Objective를 만들 때는 무조건 CreateObjective() 함수를 사용해야 함. (만든 순서와 우선순위가 동일함.)
 *    2) Behaviour를 추가 할 때는 무조건 AddBehaviour() 함수를 사용해야 함.
 *    3) SetDefaultObjective(Objective) 함수를 무조건 실행해야 함.
 * 
 */

public abstract partial class OBB_Controller<D, S> : MonoBehaviour 
    where D : OBB_Data, new()
    where S : IOBB_State
{
    #region Struct: State Action
    protected struct StateAction
    {
        public class ID { }
        private ID id;

        public Action start;
        public Action end;
        public Action update;

        public StateAction(Action start = null, Action end = null, Action update = null)
        {
            id = new ID();

            this.start = start;
            this.end = end;
            this.update = update;
        }

        public static bool operator ==(StateAction a, StateAction b)
        {
            return a.id == b.id;
        }
        public static bool operator !=(StateAction a, StateAction b)
        {
            return a.id != b.id;
        }
        public override bool Equals(object obj)
        {
            return obj is StateAction action &&
                   id == action.id &&
                   EqualityComparer<Action>.Default.Equals(start, action.start) &&
                   EqualityComparer<Action>.Default.Equals(end, action.end) &&
                   EqualityComparer<Action>.Default.Equals(update, action.update);
        }
        public override int GetHashCode()
        {
            var hashCode = 996125917;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Action>.Default.GetHashCode(start);
            hashCode = hashCode * -1521134295 + EqualityComparer<Action>.Default.GetHashCode(end);
            hashCode = hashCode * -1521134295 + EqualityComparer<Action>.Default.GetHashCode(update);
            return hashCode;
        }
    }
    #endregion

    #region Static Var: State Action
    // This is "Unique" Empty State Action
    protected static StateAction EMPTY_STATE_ACTION => new StateAction(null, null, null);
    #endregion

    #region Var: OOB
    private bool canRunLateEnter = true;
    #endregion

    #region Prop: 
    // Data
    [SerializeField]
    private D _data = new D();
    protected D data => _data;

    // State
    protected readonly OBB_State END_BEHAVIOUR = null;
    protected IOBB_State defaultState
    { get; private set; }
    protected IOBB_State currentState
    { get; private set; }
    protected StateAction defaultStateAction
    { get; private set; }
    protected StateAction currentStateAction
    { get; private set; }

    // Behaviour
    protected OBB_Behaviour currentBehaviour
    { get; private set; }

    // Objective
    protected List<Objective> objectives
    { get; private set; } = new List<Objective>();
    protected Objective defaultObjective
    { get; private set; }
    protected Objective currentObjective
    { get; private set; }
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        data.Init_Awake(gameObject);

        InitStates();
        InitBehaviours();
        InitObjectives();

        // Check Default
        if (defaultState == null)
            Debug.LogError($"{gameObject.name} > No Default State!");

        if (defaultStateAction == null)
            Debug.LogError($"{gameObject.name} > No Default State Action!");

        if (defaultObjective == null)
            Debug.LogError($"{gameObject.name} > No Default Objective!");
    }
    protected virtual void Start()
    {
        data.Init_Start(gameObject);

        // Update OBB
        OBB_Update();
    }
    protected virtual void Update()
    {
        // Update OBB
        OBB_Update();

        // Run State
        currentState.OnUpdate();
    }
    protected virtual void LateUpdate()
    {
        RunStateLateEnter();

        // Run State
        currentState.OnLateUpdate();
    }
    protected virtual void FixedUpdate()
    {
        // Run State
        currentState.OnFixedUpdate();
    }
    #endregion

    #region Method: Initialize
    // Init States
    protected abstract void InitStates();
    protected virtual void GetState<TState>(ref TState state)
        where TState : IOBB_State
    {
        state = GetComponent<TState>();

        if (state == null)
            Debug.LogError($"{gameObject.name} > Can't Find State: {typeof(TState).Name}!");

        state.InitData(data);
    }
    protected void SetDefaultState(OBB_State state, StateAction stateAction)
    {
        defaultState = state;
        defaultStateAction = stateAction;

        currentState = state;
        currentStateAction = stateAction;
    }

    // Init Behaviours
    protected abstract void InitBehaviours();

    // Init Objectives
    protected abstract void InitObjectives();
    protected Objective NewObjective(Func<bool> isPossible, bool forceRun = false)
    {
        Objective objective = new Objective(isPossible, forceRun);
        objectives.Add(objective);
        return objective;
    }
    protected Objective SetDefaultObjective()
    {
        Objective defaultObjective = new Objective(() => true);
        this.defaultObjective = defaultObjective;

        currentObjective = defaultObjective;
        return defaultObjective;
    }
    #endregion

    #region Method: Update OBB
    private (Objective objective, OBB_Behaviour behaviour, IOBB_State state, StateAction stateAction) GetNext()
    {
        // Get Next Objective
        Objective nextObjective = defaultObjective;
        for (int i = 0; i < objectives.Count; i++)
        {
            if (!objectives[i].IsPossible())
                continue;

            nextObjective = objectives[i];
            break;
        }

        // Check Current
        if (!nextObjective.ForceRun)
        {
            var current = currentObjective.GetCurrent(currentState);

            // Check Behaviour Wait
            if (current.state != null && currentBehaviour != null && currentBehaviour.Wait)
                return (currentObjective, currentBehaviour, current.state, current.stateAction);

            // Check Objective Wait
            if (current.behaviour != null && currentObjective.Wait)
                return (currentObjective, current.behaviour, current.state, current.stateAction);

            // Check Objective Transition
            if (current.behaviour == null && currentObjective.Transition != null && currentObjective.Transition.IsPossible())
            {
                var transition = currentObjective.Transition.GetCurrent(currentState);
                return (currentObjective.Transition, transition.behaviour, transition.state, transition.stateAction);
            }
        }

        // Return Next Objective
        var next = nextObjective.GetCurrent(currentState);
        return (nextObjective, next.behaviour, next.state, next.stateAction);
    }
    private void OBB_Update()
    {
        // Get Next Objective
        var next = GetNext();

        currentObjective = next.objective;

        if (currentBehaviour != next.behaviour)
        {
            currentBehaviour?.OnBehaviourEnd();
            currentBehaviour = next.behaviour;
        }

        // Run Current State
        RunState(next.state, next.stateAction);
    }

    private void RunState(IOBB_State state, StateAction stateAction)
    {
        if (state == null)
        {
            state = defaultState;
            stateAction = defaultStateAction;
        }

        // Change State
        if (currentStateAction != stateAction)
        {
            currentState?.OnExit();
            currentStateAction.end?.Invoke();

            currentState = state;
            currentStateAction = stateAction;

            currentStateAction.start?.Invoke();
            currentState.OnEnter();
            canRunLateEnter = true;
        }

        // Run State Action Update
        currentStateAction.update?.Invoke();
    }
    private void RunStateLateEnter()
    {
        if (!canRunLateEnter)
            return;

        canRunLateEnter = false;
        currentState.OnLateEnter();
    }
    #endregion
}
