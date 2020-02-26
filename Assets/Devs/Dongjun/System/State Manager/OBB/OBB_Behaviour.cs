using System;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class OBB_Controller<D, S> : MonoBehaviour
    where D : OBB_Data, new()
    where S : IOBB_State
{
    #region Class: Behaviour
    protected abstract class OBB_Behaviour
    {
        public bool Wait = false;

        public T Clone<T>(bool wait) where T : OBB_Behaviour
        {
            var result = MemberwiseClone() as T; // 안에 State들을 바꿀 일이 없기 때문에 DeepCopy 할 필요 없음.
            result.Wait = wait;
            return result;
        }

        public virtual void OnBehaviourEnd() { }
        public abstract (IOBB_State state, StateAction stateAction) GetCurrent(IOBB_State currentState);
    }
    #endregion

    #region Class: Behaviour -> Single
    protected sealed class Single : OBB_Behaviour
    {
        private (IOBB_State state, StateAction stateAction, Func<bool> done) data;

        // Ctor
        public Single(IOBB_State state, StateAction stateAction, Func<bool> done)
        {
            if (state == null)
            {
                Debug.LogError($"{state.GetType().Name} is Null!");
                return;
            }

            data = (state, stateAction, done);
        }

        // Behaviour
        public override (IOBB_State state, StateAction stateAction) GetCurrent(IOBB_State currentState)
        {
            if (data.done())
                return default;

            return (data.state, data.stateAction);
        }
    }
    #endregion

    #region Class: Behaviour -> Process
    protected sealed class Process : OBB_Behaviour
    {
        private List<(IOBB_State state, StateAction stateAction, Func<bool> done)> data;

        // Ctor
        public Process(params (IOBB_State state, StateAction stateAction, Func<bool> done)[] data)
        {
            this.data = new List<(IOBB_State state, StateAction stateAction, Func<bool> done)>();

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
        public override (IOBB_State state, StateAction stateAction) GetCurrent(IOBB_State currentState)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].done())
                    continue;

                return (data[i].state, data[i].stateAction);
            }

            return default;
        }
    }
    #endregion

    #region Class: Behaviour -> Sequence
    protected sealed class Sequence : OBB_Behaviour
    {
        private List<(IOBB_State state, StateAction stateAction, Func<bool> done)> data;
        private int curIndex = 0;

        // Ctor
        public Sequence(params (IOBB_State state, StateAction stateAction, Func<bool> done)[] data)
        {
            this.data = new List<(IOBB_State state, StateAction stateAction, Func<bool> done)>();

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
        public override void OnBehaviourEnd()
        {
            curIndex = 0;
        }
        public override (IOBB_State state, StateAction stateAction) GetCurrent(IOBB_State currentState)
        {
            if (data[curIndex].done())
            {
                if (curIndex != data.Count - 1)
                {
                    curIndex++;
                }
                else
                {
                    return default;
                }
            }

            return (data[curIndex].state, data[curIndex].stateAction);
        }
    }
    #endregion

    #region Class: Behaviour -> Continue
    protected sealed class Continue : OBB_Behaviour
    {
        private List<(IOBB_State state, StateAction stateAction, Func<bool> done)> data;
        private int curIndex = 0;

        // Ctor
        public Continue(params (IOBB_State state, StateAction stateAction, Func<bool> done)[] data)
        {
            this.data = new List<(IOBB_State state, StateAction stateAction, Func<bool> done)>();

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
        public void Restart()
        {
            curIndex = 0;
        }
        public override (IOBB_State state, StateAction stateAction) GetCurrent(IOBB_State currentState)
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
                    return default;
                }
            }

            return (data[curIndex].state, data[curIndex].stateAction);
        }
    }
    #endregion

    #region Class: Behaviour -> Choice
    protected sealed class Choice : OBB_Behaviour
    {
        private Dictionary<IOBB_State, (StateAction stateAction, Func<IOBB_State> getNext)> data;
        private (IOBB_State state, StateAction stateAction) defaultData;

        // Ctor
        public Choice(
            (IOBB_State state, StateAction stateAction, Func<IOBB_State> getNext) defaultData,
            params (IOBB_State state, StateAction stateAction, Func<IOBB_State> next)[] data)
        {
            this.data = new Dictionary<IOBB_State, (StateAction, Func<IOBB_State>)>();

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
        public override (IOBB_State state, StateAction stateAction) GetCurrent(IOBB_State currentState)
        {
            if (currentState == null || !data.ContainsKey(currentState))
            {
                IOBB_State next = data[defaultData.state].getNext();
                return (next, data[next].stateAction);
            }

            IOBB_State nextState = data[currentState].getNext();

            if (nextState == null)
                return default;

            return (nextState, data[nextState].stateAction);
        }
    }
    #endregion
}
