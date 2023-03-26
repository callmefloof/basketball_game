using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.AI.State_Machine.States.Base;
using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using UnityEditor.Experimental.GraphView;

namespace Assets.Scripts.AI.State_Machine.States
{
    public class ShouldGetCloseToHoop : State 
    {
        private Baller baller;
        private Vector3 newPos;
        private Vector3 pos;
        private Vector3 teamHoop;
        private bool isOffensive;
        

        public ShouldGetCloseToHoop(IStateMachineMember owner, bool isOffensive) : base(owner)
        {
            Owner = owner;
            this.isOffensive = isOffensive;

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

            string teamOneTarget = String.Empty;
            string teamTwoTarget = String.Empty;

            if (isOffensive)
            {
                teamOneTarget = "HoopTwo";
                teamTwoTarget = "HoopOne";
                
            }
            else
            {
                teamOneTarget = "HoopOne";
                teamTwoTarget = "HoopTwo";
            }

            switch (baller.team)
            {
                case 1:
                    teamHoop = GameObject.FindWithTag(teamOneTarget).transform.position;
                    break;
                
                case 2:
                    teamHoop = GameObject.FindWithTag(teamTwoTarget).transform.position;
                    break;
            }
            
            var step = baller.speed * Time.deltaTime;
            
           
            baller.navMeshAgent.SetDestination(teamHoop);

            Debug.Log($"Team; {baller.team} member {baller.environmentInfoComponent.Team.FindIndex(x => x == baller)} is going at  {baller.speed}");
            baller.StateMachine.ChangeState(new Examine(baller));
        }

        public override void Exit()
        {
            
        }
         
        
    }
} 

