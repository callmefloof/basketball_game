using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace statemachine
{
    public class AttackingState : State 
    {
        [SerializeField] private Vector3 basketballPosition;
        [SerializeField] private Vector3 pos;
        [SerializeField] private GameObject Baller;
        private Vector3 newPos;
        public float speed = 1.0f;
        public bool heldBall = false;
        private bool shouldMove = true; // Flag to control whether the player should move or not
        
        public AttackingState(Baller owner, StateMachine machine) : base(owner, machine)
        {
            pos = GameObject.FindWithTag("AI 1").transform.position;
        }
 
        public override void EnterState()
        {
            Baller = owner.gameObject; // Set Baller to the object that the script is attached to
            shouldMove = true;
        }

        public override void UpdateState()
        {
            
            if (!shouldMove) return; // Stop moving if shouldMove is false
            
            var step = speed * Time.deltaTime;
            basketballPosition = GameObject.FindWithTag("Ball").transform.position;
            pos = GameObject.FindWithTag("AI 1").transform.position;
            Baller.transform.position = Vector3.MoveTowards(pos, basketballPosition, step);
            
            if (heldBall) {
                speed = 0f; // stop moving if ball is held
            }

            Debug.Log(speed);
        }

        public override void ExitState()
        {
        }
         
        void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Ball")
            {
                heldBall = true;
                shouldMove = false;
                Debug.Log("Collision detected with player object!");
                Debug.Log("The ball is held? " + heldBall.ToString());
                
                
            }
        }
    }
} 

