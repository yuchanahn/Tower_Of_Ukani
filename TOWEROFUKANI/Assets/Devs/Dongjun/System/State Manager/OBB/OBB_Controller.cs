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
 *    조건이 맞다면 무조건(Behaviour: Wait 과  Objective: Transition 무시) 이 목표를 실행함.
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
 * 유의 사항:
 * 
 * -> States 초기화
 *    1) State를 가져올 때 GetComponent 대신 GetState(ref State) 함수를 사용해야함.
 *    2) SetDefaultState(State, StateAction) 함수는 무조건 실행해야 함.
 * 
 * -> Behaviour 초기화
 *    1) Single: 하나의 State만 완수하면 될 때 사용함.
 *    2) Process: 두개 이상의 State들이 순차적으로 모두 완수해야 할 때 사용함.
 *    3) Sequence: 두개 이상의 State들을 순차적으로 완수해야 할 때 사용함.
 *    3) Continue: 두개 이상의 State들을 순차적으로 완수해야 하며 다른 행동으로 바뀌었다가 돌아와도 이어서 진행 해야할 때 사용함.
 *    4) Choice: 두개 이상의 State들이 조건에 의해 다른 State로 바뀌어야 할 때 사용함.
 * 
 * -> Objective 초기화
 *    1) Behaviour를 추가 할 때는 무조건 AddBehaviour() 함수를 사용해야 함.
 *    2) SetDefaultObjective(Objective) 함수는 무조건 실행해야 함.
 *    2) AddObjectives(params Objective) 함수는 무조건 실행해야 함.
 * 
 */

public abstract class OBB_Controller<Data, State> : MonoBehaviour 
    where Data : OBB_Data, new()
    where State : OBB_State<Data>
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
    // This is Unused State Action
    private static readonly StateAction NULL_STATE_ACTION = new StateAction();

    // This is "Unique" Empty State Action
    protected static StateAction EMPTY_STATE_ACTION => new StateAction(null, null, null);
    #endregion

    #region Class: Behaviour
    protected abstract class OBB_Behaviour
    {
        public bool Wait = false;

        public T Clone<T>() where T : OBB_Behaviour
        {
            return MemberwiseClone() as T;
        }

        public virtual void OnBehaviourExit() { }
        public abstract (State state, StateAction stateAction) GetCurrent(State currentState);
    }
    #endregion

    #region Class: Behaviour -> Single
    protected sealed class Single : OBB_Behaviour
    {
        private List<(State state, StateAction stateAction, Func<bool> done)> data = 
            new List<(State, StateAction, Func<bool>)>();

        // Ctor
        public Single(State state, StateAction stateAction, Func<bool> done)
        {
            data.Add((state, stateAction, done));
        }
        public Single(params (State state, StateAction stateAction, Func<bool> done)[] data)
        {
            for (int i = 0; i < data.Length; i++)
                this.data.Add(data[i]);
        }

        // Behaviour
        public override (State state, StateAction stateAction) GetCurrent(State currentState)
        {
            if (data.Count == 1 && data[0].done())
            {
                return (data[0].state, data[0].stateAction);
            }
            else
            {
                for (int i = 0; i < data.Count; i++)
                    if (data[i].done())
                        return (data[i].state, data[i].stateAction);
            }

            return (null, NULL_STATE_ACTION);
        }
    }
    #endregion

    #region Class: Behaviour -> Process
    protected sealed class Process : OBB_Behaviour
    {
        private List<(State state, StateAction stateAction, Func<bool> done)> data;

        // Ctor
        public Process(params (State state, StateAction stateAction, Func<bool> done)[] data)
        {
            this.data = new List<(State state, StateAction stateAction, Func<bool> done)>();

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].state == null)
                {
                    Debug.LogError($"{data[i].state.GetType().Name} is Null!");
                    continue;
                }

                this.data.Add(data[i]);
            }
        }

        // Behaviour
        public override (State state, StateAction stateAction) GetCurrent(State currentState)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].done())
                    continue;

                return (data[i].state, data[i].stateAction);
            }

            return (null, NULL_STATE_ACTION);
        }
    }
    #endregion

    #region Class: Behaviour -> Sequence
    protected sealed class Sequence : OBB_Behaviour
    {
        private List<(State state, StateAction stateAction, Func<bool> done)> data;
        private int curIndex = 0;

        // Ctor
        public Sequence(params (State state, StateAction stateAction, Func<bool> done)[] data)
        {
            this.data = new List<(State state, StateAction stateAction, Func<bool> done)>();

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].state == null)
                {
                    Debug.LogError($"{data[i].state.GetType().Name} is Null!");
                    continue;
                }

                this.data.Add(data[i]);
            }
        }

        // Behaviour
        public override void OnBehaviourExit()
        {
            curIndex = 0;
        }
        public override (State state, StateAction stateAction) GetCurrent(State currentState)
        {
            if (data[curIndex].done())
            {
                if (curIndex != data.Count - 1)
                {
                    curIndex++;
                }
                else
                {
                    return (null, NULL_STATE_ACTION);
                }
            }

            return (data[curIndex].state, data[curIndex].stateAction);
        }
    }
    #endregion

    #region Class: Behaviour -> Sequence
    protected sealed class Continue : OBB_Behaviour
    {
        private List<(State state, StateAction stateAction, Func<bool> done)> data;
        private int curIndex = 0;

        // Ctor
        public Continue(params (State state, StateAction stateAction, Func<bool> done)[] data)
        {
            this.data = new List<(State state, StateAction stateAction, Func<bool> done)>();

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].state == null)
                {
                    Debug.LogError($"{data[i].state.GetType().Name} is Null!");
                    continue;
                }

                this.data.Add(data[i]);
            }
        }

        // Behaviour
        public override (State state, StateAction stateAction) GetCurrent(State currentState)
        {
            if (data[curIndex].done())
            {
                if (curIndex != data.Count - 1)
                {
                    curIndex++;
                }
                else
                {
                    curIndex = 0;
                    return (null, NULL_STATE_ACTION);
                }
            }

            return (data[curIndex].state, data[curIndex].stateAction);
        }
    }
    #endregion

    #region Class: Behaviour -> Choice
    protected sealed class Choice : OBB_Behaviour
    {
        private Dictionary<State, (StateAction stateAction, Func<State> getNext)> data;
        private (State state, StateAction stateAction) defaultData;

        // Ctor
        public Choice(
            (State state, StateAction stateAction, Func<State> getNext) defaultData, 
            params (State state, StateAction stateAction, Func<State> next)[] data)
        {
            this.data = new Dictionary<State, (StateAction, Func<State>)>();

            // Check Default Null
            if (defaultData.state == null)
            {
                Debug.LogError($"{defaultData.state.GetType().Name} is Null!");
                return;
            }

            // Add Default
            this.defaultData = (defaultData.state, defaultData.stateAction);
            this.data.Add(defaultData.state, (defaultData.stateAction, defaultData.getNext));

            // Add States
            for (int i = 0; i < data.Length; i++)
            {
                // Check Null
                if (data[i].state == null)
                {
                    Debug.LogError($"{data[i].state.GetType().Name} is Null!");
                    continue;
                }
                if (this.data.ContainsKey(data[i].state))
                    continue;

                this.data.Add(data[i].state, (data[i].stateAction, data[i].next));
            }
        }

        // Behaviour
        public override (State state, StateAction stateAction) GetCurrent(State currentState)
        {
            if (currentState == null || !data.ContainsKey(currentState))
            {
                State next = data[defaultData.state].getNext();
                return (next, data[next].stateAction);
            }

            State nextState = data[currentState].getNext();

            if (nextState == null)
                return (null, NULL_STATE_ACTION);

            return (nextState, data[nextState].stateAction);
        }
    }
    #endregion

    #region Class: Objective
    protected class Objective
    {
        protected List<OBB_Behaviour> behaviours;

        public Func<bool> IsPossible
        { get; protected set; }
        public Objective Transition
        { get; protected set; }
        public bool ForceRun
        { get; private set; } = false;
        public bool Wait
        { get; private set; } = false;

        public Objective(Func<bool> isPossible, bool forceRun = false)
        {
            behaviours = new List<OBB_Behaviour>();

            IsPossible = isPossible;
            ForceRun = forceRun;
        }

        public Objective SetTransition(Objective trnasition)
        {
            Transition = trnasition;
            Transition.Wait = true;
            return this;
        }
        public Objective AddBehaviour<T>(T behaviour, bool wait = false) where T : OBB_Behaviour
        {
            T clone = behaviour.Clone<T>();
            clone.Wait = wait;

            behaviours.Add(clone);
            return this;
        }
        public (OBB_Behaviour behaviour, State state, StateAction stateAction) GetCurrent(State current)
        {
            (OBB_Behaviour behaviour, State state, StateAction stateAction) result = (null, null, NULL_STATE_ACTION);

            for (int i = 0; i < behaviours.Count; i++)
            {
                var curData = behaviours[i].GetCurrent(current);
                if (curData.state != null)
                {
                    result.behaviour = behaviours[i];
                    result.state = curData.state;
                    result.stateAction = curData.stateAction;
                    break;
                }
            }

            return result;
        }
    }
    #endregion

    #region Var: Inspector
    [SerializeField] private Data _data = new Data();
    #endregion

    #region Var: OOB
    private bool canRunLateEnter = true;
    #endregion

    #region Prop: 
    // Data
    protected Data data => _data;

    // Objective
    protected List<Objective> objectives
    { get; private set; } = new List<Objective>();
    protected Objective defaultObjective
    { get; private set; }
    protected Objective currentObjective
    { get; private set; }

    // Behaviour
    protected OBB_Behaviour currentBehaviour
    { get; private set; }

    // State
    protected readonly State done = null;
    protected State defaultState
    { get; private set; }
    protected State currentState
    { get; private set; }
    protected StateAction defaultStateAction
    { get; private set; }
    protected StateAction currentStateAction
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
    protected void GetState<T>(ref T state) where T : State
    {
        state = GetComponent<T>();

        if (state == null)
        {
            Debug.LogError($"{gameObject.name} > Can't Find State: {typeof(T).Name}!");
            return;
        }

        state.InitData(data);
    }
    protected void SetDefaultState(State state, StateAction stateAction)
    {
        defaultState = state;
        defaultStateAction = stateAction;
    }

    // Init Behaviours
    protected abstract void InitBehaviours();

    // Init Objectives
    protected abstract void InitObjectives();
    protected Objective SetDefaultObjective()
    {
        return this.defaultObjective = new Objective(() => true);
    }
    protected void SetDefaultObjective(Objective defaultObjective)
    {
        this.defaultObjective = defaultObjective;
    }
    protected void AddObjectives(params Objective[] objectives)
    {
        for (int i = 0; i < objectives.Length; i++)
            this.objectives.Add(objectives[i]);
    }
    #endregion

    #region Method: Update OBB
    private void RunState(State state, StateAction stateAction)
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

    private Objective GetNextObjective()
    {
        Objective nextObjective = defaultObjective;

        for (int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i].IsPossible())
            {
                nextObjective = objectives[i];
                break;
            }
        }

        if (!nextObjective.ForceRun && currentObjective != null)
        {
            OBB_Behaviour curBehaviour = currentObjective.GetCurrent(currentState).behaviour;

            if (curBehaviour == null && currentObjective.Transition != null && currentObjective.Transition.IsPossible())
                return currentObjective.Transition;

            if (curBehaviour != null && currentObjective.Wait)
                return currentObjective;
        }

        return nextObjective;
    }
    private bool WaitBehaviour(Objective nextObjective)
    {
        if (nextObjective.ForceRun || currentBehaviour == null || (!currentBehaviour?.Wait ?? false))
            return false;

        var current = currentBehaviour.GetCurrent(currentState);

        if (current.state == null)
        {
            return false;
        }
        else
        {
            RunState(current.state, current.stateAction);
            return true;
        }
    }
    private void OBB_Update()
    {
        // Get Next Objective
        Objective nextObjective = GetNextObjective();

        // Wait Behaviour
        if (WaitBehaviour(nextObjective))
            return;

        // Get Next Behaviour and State
        var next = nextObjective.GetCurrent(currentState);

        currentObjective = nextObjective;

        if (currentBehaviour != next.behaviour)
        {
            currentBehaviour?.OnBehaviourExit();
            currentBehaviour = next.behaviour;
        }

        // Run Current State
        RunState(next.state, next.stateAction);
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
