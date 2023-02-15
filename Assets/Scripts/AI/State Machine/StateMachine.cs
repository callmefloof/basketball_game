using Assets.Scripts.AI.State_Machine.States.Base;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.State_Machine
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        public void ChangeState(State newState)
        {
            if (newState== null) CurrentState = null;
            if (CurrentState != null) CurrentState.Exit();
            CurrentState= newState;
            if (CurrentState != null) CurrentState.Enter();
            return;
        }

        public void Update()
        {
            if(CurrentState != null) CurrentState.Execute();
        }
        
    }
}