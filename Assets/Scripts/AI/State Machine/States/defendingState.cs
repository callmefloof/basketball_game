using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.AI.State_Machine.States.Base;
using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using System.Linq;

namespace Assets.Scripts.AI.State_Machine.States
{
    public class DefendingState : State 
    {
        private Baller baller;
        private Vector3 newPos;
        private Vector3 pos;
        private Vector3 defendingZonePosition;

        private ShootingZone shootingZone;
        public DefendingState(IStateMachineMember owner) : base(owner)
        {
            Owner = owner;
            pos = GameObject.FindWithTag("AI 1").transform.position;
        }
 
        public override void Enter()
        {
            baller = (Owner as Baller); // Set Baller to the object that the script is attached to
            shootingZone = GameObject.FindObjectsOfType<ShootingZone>().Where(x => x.tag == (baller.team == 1 ? "ZoneOne" : "ZoneTwo")).First();
            if (shootingZone == null) throw new NotImplementedException();
            baller.shouldMove = true;
        }

        public override void Execute()
        {

            //if (!baller.shouldMove)
            //{
            //    baller.StateMachine.ChangeState(new Examine(baller));
            //    return; // Stop moving if shouldMove is false
            //}

            //switch (baller.team)
            //{
            //    case 1:
            //        defendingZonePosition = GameObject.FindWithTag("ZoneOne").transform.position;
            //        break;
                
            //    case 2:
            //        defendingZonePosition = GameObject.FindWithTag("ZoneTwo").transform.position;
            //        break;
            //}
            
            //var step = baller.speed * Time.deltaTime;
            
           
            //baller.navMeshAgent.SetDestination(defendingZonePosition);


            //if (baller.heldBall) {
            //    baller.speed = 0f; // stop moving if ball is held
            //}

            //Debug.Log($"Team; {baller.team} member {baller.environmentInfoComponent.Team.FindIndex(x => x == baller)} is going at  {baller.speed}");

            if(baller.environmentInfoComponent.EnemyTeam.Any(x=> x.heldBall && shootingZone.BallersInZone.Contains(x)))
            {
                baller.StateMachine.ChangeState(new AttackingState(baller));
                return;
            }
            else
            {
                baller.StateMachine.ChangeState(new ShouldGetCloseToHoop(baller, false));
                return;
            }


            baller.StateMachine.ChangeState(new Examine(baller));
        }

        public override void Exit()
        {
            
        }
         
        
    }
} 

