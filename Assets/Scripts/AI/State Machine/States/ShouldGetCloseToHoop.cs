using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.AI.State_Machine.States.Base;
using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using UnityEditor.Experimental.GraphView;
using JetBrains.Annotations;
using System.Linq;

namespace Assets.Scripts.AI.State_Machine.States
{
    public class ShouldGetCloseToHoop : State 
    {
        private Baller baller;
        private Vector3 newPos;
        private Vector3 pos;
        private Vector3 hoopPosition;
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
                    hoopPosition = GameObject.FindWithTag(teamOneTarget).transform.position;
                    break;
                
                case 2:
                    hoopPosition = GameObject.FindWithTag(teamTwoTarget).transform.position;
                    break;
            }
            
            var step = baller.speed * Time.deltaTime;
            List<Vector3> enemyPositions = new List<Vector3>();

            baller.environmentInfoComponent.EnemyTeam.ForEach(x => enemyPositions.Add(x.transform.position));
            Vector3 averagedEnemyPosition = new Vector3(enemyPositions.Average(x => x.x), enemyPositions.Average(x => x.y), enemyPositions.Average(x => x.z));
            Vector3 newPosition = Vector3.Lerp(hoopPosition, averagedEnemyPosition, baller.Defensiveness);
            Debug.DrawLine(newPosition, new Vector3(newPosition.x, 10000, newPosition.z));



            baller.navMeshAgent.SetDestination(newPosition);

            Debug.Log($"Team; {baller.team} member {baller.environmentInfoComponent.Team.FindIndex(x => x == baller)} is going at  {baller.speed}");
            baller.StateMachine.ChangeState(new Examine(baller));
        }

        public override void Exit()
        {
            
        }
         
        
    }
} 

