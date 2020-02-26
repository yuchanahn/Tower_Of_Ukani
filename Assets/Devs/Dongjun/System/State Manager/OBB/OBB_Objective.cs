using System;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class OBB_Controller<D, S> : MonoBehaviour
    where D : OBB_Data, new()
    where S : IOBB_State
{
    protected class Objective
    {
        private List<OBB_Behaviour> behaviours = new List<OBB_Behaviour>();

        public Func<bool> IsPossible
        { get; protected set; }
        public Objective Transition
        { get; protected set; } = null;
        public bool ForceRun
        { get; private set; } = false;
        public bool Wait
        { get; private set; } = false;

        // Ctor
        public Objective(Func<bool> isPossible, bool forceRun = false)
        {
            IsPossible = isPossible;
            ForceRun = forceRun;
        }

        // Init
        public Objective AddBehaviour<T>(T behaviour, bool wait = false) where T : OBB_Behaviour
        {
            behaviours.Add(behaviour.Clone<T>(wait));
            return this;
        }
        public Objective SetTransition(Func<bool> isPossible, bool forceRun = false)
        {
            Transition = new Objective(isPossible, forceRun);
            Transition.Wait = true;
            return Transition;
        }

        // Get Current
        public (OBB_Behaviour behaviour, IOBB_State state, StateAction stateAction) GetCurrent(IOBB_State currentState)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                var current = behaviours[i].GetCurrent(currentState);
                if (current.state != null)
                    return (behaviours[i], current.state, current.stateAction);
            }

            return default;
        }
    }
}
