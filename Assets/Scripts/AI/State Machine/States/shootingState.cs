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
        private Vector3 shootposition;
        private Vector3 target;
        
        public ShootingState(IStateMachineMember owner, Vector3 target): base(owner)
        {
            baller = owner as Baller;
            this.target = target;
        }

        public override void Enter()
        {
            ballObject = GameObject.Find("Ball").GetComponent<Ball>();
            baller.shoot = false;
            shootposition = baller.team switch { 1 => GameObject.FindGameObjectWithTag("ZoneTwo").transform.GetChild(0).position, 2 => GameObject.FindGameObjectWithTag("ZoneOne").transform.GetChild(0).position, _ => throw new System.NotImplementedException() };
            
            Debug.Log("Switched to ShootingState");
        }

        public override void Execute()
        {

            //old version
            //if (baller.shoot)
            //{
            //    var targetHoop = baller.environmentInfoComponent.EnemyHoop.transform.position;
            //    //Level Y to the baller so we don't tilt
            //    var targetHoopXZ = new Vector3(targetHoop.x, baller.transform.position.y, targetHoop.z);
            //    baller.navMeshAgent.updateRotation = false;
            //    baller.navMeshAgent.updatePosition = false;
            //    baller.transform.LookAt(targetHoopXZ);
            //    Debug.Log(baller.shoot);
            //    ballObject.StartCoroutine(ballObject.ShootBall());
            //    baller.navMeshAgent.updateRotation = true;
            //    baller.navMeshAgent.updatePosition = true;
            //    baller.shoot = false;
            //}
            //else
            //{
            //    baller.navMeshAgent.destination = shootposition;
            //}

            
            //Level Y to the baller so we don't tilt
            var targetXZ = new Vector3(target.x, baller.transform.position.y, target.z);
            baller.navMeshAgent.updateRotation = false;
            baller.navMeshAgent.updatePosition = false;
            baller.transform.LookAt(targetXZ);
            Debug.Log(baller.shoot);
            ballObject.StartCoroutine(ballObject.ShootBall(target));
            baller.navMeshAgent.updateRotation = true;
            baller.navMeshAgent.updatePosition = true;


            baller.StateMachine.ChangeState(new Examine(baller));
        }

        
        public override void Exit()
        {
            
        }
    }
}