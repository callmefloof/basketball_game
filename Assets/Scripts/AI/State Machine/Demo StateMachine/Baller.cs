    using System;
using System.Collections;
using System.Collections.Generic;
using statemachine;
using UnityEngine;
using UnityEngine.Events;

public class Baller : MonoBehaviour
{

    public UnityEvent<float> Event;
    public bool heldBall = false;
    
    private StateMachine stateMachine;
    
    private void Awake()
    {
        
        stateMachine = new StateMachine(this);
    }
    
    void Update()
    {
        if (stateMachine != null)
        {
            stateMachine.UpdateState();
        }
    }


}

