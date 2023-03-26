using System;
using System.Collections;
using Assets.Scripts.AI.Environment;
using Assets.Scripts.AI.State_Machine.States;
using Assets.Scripts.Objects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Assets.Scripts.AI.State_Machine.Demo_StateMachine
{
    public class Baller : MonoBehaviour, IStateMachineMember
    {
        public float speed = 200f;
        public bool shouldMove = true; // Flag to control whether the player should move or not
        public float step = 0f;
        public UnityEvent<float> Event;
        public bool heldBall = false;
        public bool attackingSide = false;
        private Ball ball;
        public NavMeshAgent navMeshAgent;
        public NavMeshObstacle navMeshObstacle;
        public int team = 1;
        public EnvironmentInfoComponent environmentInfoComponent;
        public bool shoot = false;
        public StateMachine StateMachine { get; private set; }
       

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshObstacle = GetComponent<NavMeshObstacle>();
            StateMachine = new StateMachine();
            environmentInfoComponent = new EnvironmentInfoComponent(this);
            ball = FindFirstObjectByType<Ball>();
            shoot = false;
        }

        void Start()
        {
            if (StateMachine.CurrentState == null)
            {
                StateMachine.ChangeState(new Examine(this));
            }
        }
        
        void Update()
        {
            environmentInfoComponent.UpdateInfo();
            
            if (StateMachine != null)
            {
                StateMachine.Update();
            }
 
            step = speed * Time.deltaTime; // calculate distance to move
            navMeshAgent.speed = step;
            
            //Debug.Log("Team " +team + ": "+ environmentInfoComponent.BallerSuggestion);
            Debug.Log($"Team; {team} member {environmentInfoComponent.Team.FindIndex(x => x == this)} is doing {environmentInfoComponent.BallerSuggestion}");
        }
        
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Ball" && !collision.gameObject.GetComponent<Ball>().isBeingShot)
            {
                heldBall = true;
                shouldMove = false;
                Debug.Log("Collision detected with player object!");
                Debug.Log("The ball is held? " + heldBall.ToString());
                ball.PickUp(this);
                
            }

            if (collision.gameObject.tag == "Attacking Side")
            {
                attackingSide = true; 
            }
            
            if (collision.gameObject.tag == "ShootingZone")
            {
                Debug.Log("Shooting Collision detected with player object!");
                shoot = true; 
            }   
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "ShootingZone")
            {
                Debug.Log("Shooting Collision detected with player object!");
                shoot = false; 
            }   
        }

        

        void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.tag == "Ball" && !collision.gameObject.GetComponent<Ball>().isBeingShot)
            {
                heldBall = true;
                shouldMove = false;
                Debug.Log("Collision detected with player object!");
                Debug.Log("The ball is held? " + heldBall.ToString());
                ball.PickUp(this);
            }
        }

       
    }
}

