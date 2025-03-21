using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Assets.Scripts.AI.State_Machine.States.Base
{
    public abstract class State
    {
        public IStateMachineMember Owner { get; protected set; }

        protected State(IStateMachineMember owner)
        {
            Owner = owner;
        }
        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
    }
}
