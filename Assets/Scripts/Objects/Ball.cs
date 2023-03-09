using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts.AI.State_Machine;
using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Ball : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] public Baller ballHeldBy;
        [SerializeField] public bool isBeingHeld = false;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private Transform _dribbleTransform;
        [SerializeField] private Vector3 _floorPosition;
        [SerializeField] private bool _isHittingFloor = false;
        [SerializeField] private float _dribbleTime = 5f;
        private SphereCollider _sphereCollider;
        [SerializeField] private float _dribbleAmplitude = 0.8f;
        [SerializeField] private bool _drop = false;

        void Start()
        {
            _floorPosition = GameObject.FindGameObjectWithTag("Floor").transform.position;
            _rigidBody = GetComponent<Rigidbody>();
            _sphereCollider = GetComponent<SphereCollider>();
            StartCoroutine(Dribble());
        }

        // Update is called once per frame
        void Update()
        {
            if (!isBeingHeld) return;
            if (ballHeldBy == null) return;
            if (_drop)
            {
                Drop();
                _drop = false;
            }
        }

        public void PickUp(IStateMachineMember baller)
        {
            Drop();

            ballHeldBy = baller as Baller;
            if (ballHeldBy == null) return;
            _dribbleTransform = ballHeldBy.transform.GetChild(0);
            isBeingHeld = true;
            _rigidBody.isKinematic = true;

        }

        public void SetTrajectory(Vector3 direction, float angle, float force)
        {
            float gravity = Physics.gravity.magnitude;
            float radianAngle = angle * Mathf.Deg2Rad;
            float height = Mathf.Sin(radianAngle) * force * force / (2.0f * gravity);
            float distance = Mathf.Cos(radianAngle) * force * force / gravity;
            Vector3 velocity = direction * distance + Vector3.up * height;
            velocity = velocity.normalized * force;

            GetComponent<Rigidbody>().velocity = velocity;
        }

        private void SetDribbleConstrains(bool constrained)
        {
            if (constrained)
            {
                _rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }
            else
            {
                _rigidBody.constraints = RigidbodyConstraints.None;
            }
        }

        public void Drop()
        {
            ballHeldBy = null;
            isBeingHeld = false;
            _rigidBody.isKinematic = false;
            _dribbleTransform = null;
            return;
        }

        private IEnumerator Dribble()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                if (!isBeingHeld)
                {
                    continue;
                }

                Vector3 landingSpot = new Vector3(_dribbleTransform.position.x,
                    _floorPosition.y + _sphereCollider.radius / 2, _dribbleTransform.position.z);
                _rigidBody.position = landingSpot + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * _dribbleTime));
            }
        }

        void OnCollisionEnter(Collision collision)
        {

            if (collision.collider.tag == "hoop")
            {
                _rigidBody.isKinematic = false;
            }

            if (collision.collider.tag != "floor") return;
            if (isBeingHeld && ballHeldBy != null)
            {
                _isHittingFloor = true;
            }


        }

        void OnCollisionExit(Collision other)
        {
            if (other.collider.tag != "floor") return;
            if (isBeingHeld && ballHeldBy != null)
            {
                _isHittingFloor = false;
            }
        }
    }
}
