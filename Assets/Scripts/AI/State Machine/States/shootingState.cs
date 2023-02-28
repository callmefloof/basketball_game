using Assets.Scripts.AI.State_Machine.States.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI.State_Machine.States
{

    public class ShootingState : State
    {
        private Baller baller;
        public ShootingState(IStateMachineMember owner): base(owner)
        {
            baller = owner as Baller;
        }

        public override void Enter()
        {
            Debug.Log("Switched to ShootingState");
        }

        public override void Execute()
        {
            baller.StateMachine.ChangeState(new Examine(baller));
        }

        public override void Exit()
        {
            
        }
    }
}