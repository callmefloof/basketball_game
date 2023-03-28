using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using Assets.Scripts.AI.State_Machine.States;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

namespace Assets.Scripts.AI.Environment
{
    public class ReflexBehavior
    {

        public float currentJumpHeight { get; private set; } = 0f;
        private float jumpVelocity = 10f;
        private Vector3 BallPosition = Vector3.zero;
        private bool jumpingLocked = false;
        private bool jumpCompleted = false;
        private bool reachedJumpPeak = false;
        private float CustomJumpHeight { get; set; } = 0f;
        public Baller Owner;
        private BallerHitBox hitBox;
        private Rigidbody hitBoxRigidBody;
        private bool performJump = false;
        public ReflexBehavior (float velocity, Baller owner)
        {
            this.jumpVelocity = velocity;
            Owner = owner;
            var body = Owner.transform.GetChild(0);
            float yScale = body.transform.lossyScale.y;
            BallPosition = Owner.environmentInfoComponent.Ball.transform.position;
            CustomJumpHeight = BallPosition.y > yScale ? yScale : CustomJumpHeight;
            hitBox = Owner.transform.GetChild(0).GetComponent<BallerHitBox>();
            hitBoxRigidBody = hitBox.gameObject.GetComponent<Rigidbody>();
        }

        public void Update()
        {
            JumpCheck();
        }



        private void JumpCheck()
        {
            if (!Owner.environmentInfoComponent.Ball.isBeingShot) return;
            Vector3 ballPositionXZ = new Vector3(BallPosition.x, 0f, BallPosition.z);
            Vector3 ballerPostionXZ = new Vector3(Owner.transform.position.x, 0f, Owner.transform.position.z);
            float angle = Vector3.Angle(ballPositionXZ, ballerPostionXZ);
            float distance = Vector3.Distance(ballerPostionXZ, ballPositionXZ);
            if (Owner.environmentInfoComponent.Ball.BallShotBy == Owner || Owner.receivingPass) return;
            if (Owner.team == Owner.environmentInfoComponent.Ball.BallShotBy.team) return;
            Vector3 relativePositionXZ = ballPositionXZ - ballerPostionXZ;
            bool isInFront = Vector3.Dot(Owner.transform.forward, relativePositionXZ) > 0.0f;


            if (!isInFront) return;
            if (angle > 15f && angle < -15f) return;
            if (distance > 10f) return;

            if (!jumpingLocked)
            {
                
                Owner.StartCoroutine(PeformJump());
                Owner.StartCoroutine(JumpLock());
                return;
            }

        }
        private IEnumerator PeformJump()
        {
            if (!performJump)
            {
                performJump = true;

                if (hitBox == null) yield return null;
                float curJumpHeight = 0f;
                


                //curJumpHeight never reaches zero or the peak exactly, therefore we use an approximation within a range of 0.01f
                while (!jumpCompleted)
                {
                    curJumpHeight = Mathf.Abs(Mathf.Sin(Time.fixedTime * Mathf.PI)) * CustomJumpHeight;
                    hitBox.transform.localPosition = Vector3.up * curJumpHeight;
                    reachedJumpPeak = reachedJumpPeak ? reachedJumpPeak : curJumpHeight > CustomJumpHeight-0.01f && !reachedJumpPeak;
                    jumpCompleted = curJumpHeight < 0.01f && reachedJumpPeak;
                    Debug.Log(curJumpHeight);
                    yield return new WaitForFixedUpdate();
                }

                hitBox.transform.localPosition = Vector3.zero;
                Debug.Log("Jumped");
                performJump = false;
                yield return null;
                
            }
            
        }

        private IEnumerator JumpLock()
        {
            while (!Owner.environmentInfoComponent.Ball.IsHittingFloor && Owner.environmentInfoComponent.Ball.ballHeldBy == null )
            {
                jumpingLocked = true;
                
                yield return new WaitForFixedUpdate();
            }

            while (Owner.environmentInfoComponent.Ball.isBeingShot)
            {
                jumpingLocked = true;
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(10f);
            jumpingLocked = false;
            hitBox.transform.localPosition = Vector3.zero;
            
            
            yield return 0;
        }

    }
}
