using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingZone : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Baller> BallersInZone = new List<Baller>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "AI 1") return;
        Baller baller = other.gameObject.GetComponent<Baller>();
        if(!BallersInZone.Contains(baller))BallersInZone.Add(baller);
        if (baller == null) return;

        if (baller.heldBall) baller.shoot = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "AI 1") return ;
        Baller baller = other.gameObject.GetComponent<Baller>();

        if (BallersInZone.Contains(baller)) BallersInZone.Remove(baller);

    }
}
