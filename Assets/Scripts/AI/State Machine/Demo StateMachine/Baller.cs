using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.AI.State_Machine;
using Assets.Scripts.AI.State_Machine.States.Base;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class Baller : MonoBehaviour, IStateMachineMember
{
    public float speed = 200f;
    public bool shouldMove = true; // Flag to control whether the player should move or not
    public float step = 0f;
    public UnityEvent<float> Event;
    public bool heldBall = false;
    private Ball ball;
    public NavMeshAgent navMeshAgent;
    public StateMachine StateMachine { get; private set; }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        StateMachine = new StateMachine();
        ball = FindFirstObjectByType<Ball>();
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
        if (StateMachine != null)
        {
            StateMachine.Update();
        }
 
        step = speed * Time.deltaTime; // calculate distance to move
        navMeshAgent.speed = step;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            heldBall = true;
            shouldMove = false;
            Debug.Log("Collision detected with player object!");
            Debug.Log("The ball is held? " + heldBall.ToString());
            ball.PickUp(this);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            heldBall = true;
            shouldMove = false;
            Debug.Log("Collision detected with player object!");
            Debug.Log("The ball is held? " + heldBall.ToString());
            ball.PickUp(this);
        }
    }
}

