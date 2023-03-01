using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.AI.State_Machine;
using Assets.Scripts.AI.State_Machine.States.Base;
using UnityEngine;
using UnityEngine.Events;
public class Baller : MonoBehaviour, IStateMachineMember
{
    public float speed = 1.0f;
    public bool shouldMove = true; // Flag to control whether the player should move or not
    public float step = 0f;
    public UnityEvent<float> Event;
    public bool heldBall = false;
    private Ball ball;
    public StateMachine StateMachine { get; private set; }

    private void Awake()
    {
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
}

