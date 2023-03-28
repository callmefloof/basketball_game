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
        public bool receivingPass = false;
        public Vector3 receivedPassDestination = Vector3.zero;
        public ReflexBehavior ReflexBehavior { get; private set; }
        public StateMachine StateMachine { get; private set; }
        private MeshRenderer ballHeldMarker;
        public Color ballerColor;
        [SerializeField, Range(0.0f, 1.0f)] private float _aggression = 0.5f;
        public float Aggression
        {
            get => _aggression;
            set => _aggression = value > 1 ? 1 : value < 0 ? 0 : value;
        }

        [SerializeField, Range(0.0f, 1.0f),] private float _defensiveness = 0.5f;
        public float Defensiveness
        {
            get => _defensiveness;
            set => _defensiveness = value > 1 ? 1 : value < 0 ? 0 : value;
        }

        public float JumpVelocity = 2f;

        private void Awake()
        {

            ballHeldMarker = transform.GetChild(0).Find("BallHolding").GetComponent<MeshRenderer>();
            if (ballHeldMarker == null) throw new NotImplementedException();

            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshObstacle = GetComponent<NavMeshObstacle>();
            StateMachine = new StateMachine();
            environmentInfoComponent = environmentInfoComponent == null ? new EnvironmentInfoComponent(this) : environmentInfoComponent;
            ball = FindFirstObjectByType<Ball>();
            shoot = false;

            
            var meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
            var material = meshRenderer.material;
            material.SetColor("_BallerTeamColor", team == 1 ? GameManager.Instance.TeamOneColor : GameManager.Instance.TeamTwoColor);
            meshRenderer.material = material;
            ReflexBehavior = new ReflexBehavior(JumpVelocity, this);
            

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
            ballHeldMarker.enabled = heldBall;

            receivingPass = receivingPass && (!ball.IsHittingFloor && (!ball.ballHeldBy == this && ball.ballHeldBy == null)) ? receivingPass : false;

            environmentInfoComponent.UpdateInfo();
            if (ReflexBehavior != null) ReflexBehavior.Update();
            if (StateMachine != null) StateMachine.Update();
            //NOTE: Removed Obscale Component, this makes the AI much slower
            step = speed *2f * Time.deltaTime;
            navMeshAgent.speed = step;
            //Check to see if we are actually holding the ball, this is more of a dirty fallback in case the check breaks.
            heldBall = heldBall ? environmentInfoComponent.Ball.ballHeldBy == this : heldBall;


            //Debug.Log("Team " +team + ": "+ environmentInfoComponent.BallerSuggestion);
            Debug.Log($"Team; {team} member {environmentInfoComponent.Team.FindIndex(x => x == this)} is doing {environmentInfoComponent.BallerSuggestion}");
        }
        
        
    }
}

