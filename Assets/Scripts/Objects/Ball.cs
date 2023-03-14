using System.Collections;
using Assets.Scripts.AI.State_Machine;
using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
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
        private Vector3 originalPos;
        private Quaternion originalRot;

        void Start()
        {
            originalPos = transform.position;
            originalRot = transform.rotation;
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
        public bool test = false;
        public bool isBeingShot = false;
        public IEnumerator SetTrajectory(Vector3 direction, float angle, float force)
        {
            //float gravity = Physics.gravity.magnitude;
            //float radianAngle = angle * Mathf.Deg2Rad;
            //float height = Mathf.Sin(radianAngle) * force * force / (2.0f * gravity);
            //float distance = Mathf.Cos(radianAngle) * force * force / gravity;
            //Vector3 velocity = direction * distance + Vector3.up * height;
            //velocity = velocity.normalized * force;

            //GetComponent<Rigidbody>().velocity = velocity;
            
            if (!test)
            {
                
                
                Drop();
                
                isBeingShot = true;
                Vector3 ballXZPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                var enemyHoopPos = ballHeldBy.environmentInfoComponent.EnemyHoop.transform.position;
                Vector3 targetXZPos = new Vector3(enemyHoopPos.x, transform.position.y, enemyHoopPos.z);
                transform.LookAt(targetXZPos);
                double R = Vector3.Distance(ballXZPos, targetXZPos);
                //angle = Mathf.Atan2((enemyHoopPos.y - transform.position.y), (enemyHoopPos.x - transform.position.x)) * Mathf.Rad2Deg+10;
                angle = Random.Range(40f, 70f);
                double G = Physics.gravity.y;
                double tanAlpha = Mathf.Tan(angle * Mathf.Deg2Rad);
                double H = (enemyHoopPos.y) - transform.position.y;
                double step1 = G * R * R;
                double step2 = (2.0f * (H - R * tanAlpha));
                double step3 = step1 / step2;

                double Vz = (float)System.Math.Sqrt(Mathf.Abs((float)step3));

                double Vy = tanAlpha * Vz;
                var localVel = new Vector3(0f, (float)Vy, (float)Vz);
                var globalVel = transform.TransformDirection(localVel);
                
                _rigidBody.velocity = globalVel;
                Debug.Log(globalVel);
                yield return new WaitForSeconds(1f);
                isBeingShot = false;
                
            }
            

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
            //ballHeldBy = null;
            isBeingHeld = false;
            if (ballHeldBy != null) ballHeldBy.heldBall = false;
            _rigidBody.isKinematic = false;
            //_dribbleTransform = null;
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
            if (collision.collider.tag == "BallReset")
            {
                transform.SetPositionAndRotation(originalPos, originalRot);
            }

            if (collision.collider.tag == "Hoop")
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
