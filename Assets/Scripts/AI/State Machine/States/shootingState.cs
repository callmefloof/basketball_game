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
            
            //Call the drop function from the ball script   
            
            ballObject.Drop();
            
            // get the position of the hoop
            
            hoopPosition = GameObject.FindWithTag("Hoop").transform.position;
            
            //Calculate the distance of the ball to the hoop

            float distance = Vector3.Distance(baller.transform.position, hoopPosition);
            
            //Calculate the direction to the hoop
            
            Vector3 direction = hoopPosition - baller.transform.position;
            
            //Add some randomness to the throw so its not always the same
            
            direction += Random.insideUnitSphere * distance * 0.5f;
            
            //Add a vertical component to the direction from the player's distance to the hoop
            
            //Clamp is used to ensure the force and height are within reasonable limits 

            float height = Mathf.Clamp(distance * 0.2f, 0.2f, 1f);
            direction += Vector3.up * height;    

            //Normalise function in order to normalise the vector
            
            direction.Normalize();
            
            // Set the force of the throw

            float force = Mathf.Clamp(distance * 0.1f, 0.1f, 1f);
            
            // Create a angle based on the distance of the player to the hoop

            float angle = Mathf.Atan2((hoopPosition.y - baller.transform.position.y), distance) * Mathf.Rad2Deg;
            
            // Call Trajectory Function in Ball script 

            ballObject.SetTrajectory(direction, angle, force);
            
            //In order to make it more like a basketball shot we need to apply some gravity 

            //float gravity = Physics.gravity.y;
            
           // ballRigidbody.AddForce(Vector3.down * gravity * ballRigidbody.mass * 0.1f, ForceMode.Force);

            baller.StateMachine.ChangeState(new Examine(baller));
        }

        public override void Exit()
        {
            
        }
    }
}