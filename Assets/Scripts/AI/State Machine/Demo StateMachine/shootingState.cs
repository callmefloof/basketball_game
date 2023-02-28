using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace statemachine
{

    public class ShootingState : State {

      
        public ShootingState(Baller owner, StateMachine machine): base(owner, machine)
        {
            
  
        }

    public override void EnterState()
        {
            Debug.Log("Switched to ShootingState");
        }

        public override void UpdateState()
        {
        }

         public override void ExitState()
        {

        }
    }
}