using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using Assets.Scripts.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // if we fail the collision check on the baller itself, we'll try to detect it through the ball
        Baller baller = null;
        if(other.gameObject.GetComponent<Baller>() == null && other.tag != "AI 1")
        {
            if (other.gameObject.tag == "Ball" && other.gameObject.GetComponent<Ball>().isBeingHeld)
            {
                baller = other.gameObject.GetComponent<Ball>().ballHeldBy;
            }
        }
        else
        {
            baller = other.gameObject.GetComponent<Baller>();
        }
        if (baller == null) return;
        bool correctZone = (tag == "ZoneOne" && baller.team == 2) || (tag == "ZoneTwo" && baller.team == 1);
        baller.shoot = baller.heldBall == true && correctZone;
    }
}
