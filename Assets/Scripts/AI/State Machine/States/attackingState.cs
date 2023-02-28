using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.AI.State_Machine.States.Base;

namespace Assets.Scripts.AI.State_Machine.States
{
    public class AttackingState : State 
    {
        private Baller baller;
        private Vector3 newPos;
        private Vector3 pos;
        private Vector3 basketballPosition;

        public AttackingState(IStateMachineMember owner) : base(owner)
        {
            Owner = owner;
            pos = GameObject.FindWithTag("AI 1").transform.position;
        }
 
        public override void Enter()
        {
            baller = (Owner as Baller); // Set Baller to the object that the script is attached to
            baller.shouldMove = true;
        }

        public override void Execute()
        {

            if (!baller.shouldMove)
            {
                baller.StateMachine.ChangeState(new Examine(baller));
                return; // Stop moving if shouldMove is false
            }
        
            
            var step = baller.speed * Time.deltaTime;
            basketballPosition = GameObject.FindWithTag("Ball").transform.position;
            pos = GameObject.FindWithTag("AI 1").transform.position;
            baller.transform.position = Vector3.MoveTowards(pos, basketballPosition, step);
            
            if (baller.heldBall) {
                baller.speed = 0f; // stop moving if ball is held
                baller.StateMachine.ChangeState(new Examine(baller));
            }

            Debug.Log(baller.speed);
        }

        public override void Exit()
        {
            
        }
         
        
    }
} 

