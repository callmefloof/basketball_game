using Assets.Scripts.AI.State_Machine.States.Base;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using UnityEngine;
using Assets.Scripts.Objects;

namespace Assets.Scripts.AI.State_Machine.States
{

    public class ShootingState : State
    {
        private Baller baller;
        private Vector3 hoopPosition;
        private Ball ballObject;

        
        public ShootingState(IStateMachineMember owner): base(owner)
        {
            baller = owner as Baller;
        }

        public override void Enter()
        {
            ballObject = GameObject.Find("Ball").GetComponent<Ball>();
            
            Debug.Log("Switched to ShootingState");
        }

        public override void Execute()
        {
            
           
            var targetHoop = baller.environmentInfoComponent.EnemyHoop.transform.position;
            //Level Y to the baller so we don't tilt
            var targetHoopXZ = new Vector3(targetHoop.x, baller.transform.position.y, targetHoop.z);
            baller.navMeshAgent.updateRotation = false;
            baller.navMeshAgent.updatePosition = false;
            baller.transform.LookAt(targetHoopXZ);

            ballObject.StartCoroutine(ballObject.ShootBall());

            baller.navMeshAgent.updateRotation = true;
            baller.navMeshAgent.updatePosition = true;
            baller.StateMachine.ChangeState(new Examine(baller));
        }

        public override void Exit()
        {
            
        }
    }
}