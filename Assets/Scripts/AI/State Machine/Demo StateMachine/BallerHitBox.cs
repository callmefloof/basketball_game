using Assets.Scripts.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI.State_Machine.Demo_StateMachine
{
    public class BallerHitBox : MonoBehaviour
    {
        // Start is called before the first frame update
        private Baller parent;
        void Start()
        {
            parent = transform.parent.GetComponent<Baller>();
        }

        // Update is called once per frame
        void Update()
        {

        }


        void OnCollisionEnter(Collision collision)
        {
            BallCheck(collision);

            if (collision.gameObject.tag == "Attacking Side")
            {
                parent.attackingSide = true;
            }

            if (collision.gameObject.tag == "ShootingZone")
            {
                Debug.Log("Shooting Collision detected with player object!");
                parent.shoot = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "ShootingZone")
            {
                Debug.Log("Shooting Collision detected with player object!");
                parent.shoot = false;
            }
        }



        void OnCollisionStay(Collision collision)
        {
            BallCheck(collision);
            if (collision.gameObject.tag == "ShootingZone")
            {
                Debug.Log("Shooting Collision detected with player object!");
                parent.shoot = true;
            }
        }

        private void BallCheck(Collision collision)
        {
            if (collision.gameObject.tag != "Ball") return;
            Ball b = collision.gameObject.GetComponent<Ball>();
            if (b == null) return;
            if (b.isBeingShot) return;
            if (b.BallHasGraceTime) return;
            if (b.ballHeldBy == this) return;

            parent.heldBall = true;
            parent.shouldMove = false;
            Debug.Log("Collision detected with player object!");
            Debug.Log("The ball is held? " + parent.heldBall.ToString());
            parent.environmentInfoComponent.Ball.PickUp(parent);


        }
    }
}
