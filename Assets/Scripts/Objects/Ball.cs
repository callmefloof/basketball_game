using System.Collections;
using System.Linq;
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
        [field: SerializeField] public bool IsHittingFloor { get; private set; } = false;
        [SerializeField] private float _dribbleTime = 5f;
        private SphereCollider _sphereCollider;
        [SerializeField] private float _dribbleAmplitude = 0.8f;
        [SerializeField] private bool _drop = false;
        private Vector3 originalPos;
        private Quaternion originalRot;
        public float CurrentGraceTime = 0f;
        public float GraceTime = 1.5f;
        public bool BallHasGraceTime = false;
        private float ballStuckTime = 10f;
        private float currentStuckTime = 0f;

        [field: SerializeField] public Baller BallShotBy { get; private set; }

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
            //if the ball is stuck for longer than 10 seconds, reset the timer and ball
            if(currentStuckTime > ballStuckTime)
            {
                transform.position = originalPos;
                _rigidBody.velocity = Vector3.zero;
                currentStuckTime = 0f;
            }
            // the ball's grace time, prevents players from regrabbing the ball for Gracetime seconds
            if (BallHasGraceTime)
            {
                CurrentGraceTime += Time.deltaTime;
                if (CurrentGraceTime > GraceTime)
                {
                    BallHasGraceTime = false;
                    CurrentGraceTime = 0f;
                }

            }
            //Drop test
            if (_drop)
            {
                Drop();
                _drop = false;
            }
            
            

        }

        public void PickUp(IStateMachineMember baller)
        {
            if (BallHasGraceTime) return;

            // Drop();

            ballHeldBy = baller as Baller;
            if (ballHeldBy == null) return;
            _dribbleTransform = ballHeldBy.transform.GetChild(0).GetChild(0);
            isBeingHeld = true;
            _rigidBody.isKinematic = true;
            BallHasGraceTime = true;

        }
        //serves no purpose, I just don't want to remove the if statement
        public bool test = false;
        public bool isBeingShot = false;
        public bool isBeingDropped = false;
        public IEnumerator ShootBall(Vector3 target)
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

                BallShotBy = ballHeldBy;
                Drop();
                transform.position = _dribbleTransform.position;
                
                isBeingShot = true;
                Vector3 ballXZPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                //var enemyHoopPos = ballHeldBy.environmentInfoComponent.EnemyHoop.transform.position;
                Vector3 targetXZPos = new Vector3(target.x, transform.position.y, target.z);
                transform.LookAt(targetXZPos);
                float R = Vector3.Distance(ballXZPos, targetXZPos);
                float H = (target.y) - transform.position.y;
                //float anglePlayerHoop = Mathf.Atan2((enemyHoopPos.y + 10f - transform.position.y), (enemyHoopPos.x+2f - transform.position.x)) * Mathf.Rad2Deg;

                float angle = Mathf.Atan(H / R + Mathf.Sqrt((H*H)/(R*R) +1)) * Mathf.Rad2Deg;
                //angle = Random.Range(40f, 70f);
                //
                //angle = (float)((2f/17f) * (90f + anglePlayerHoop) * R);
                double G = Physics.gravity.y;
                double tanAlpha = Mathf.Tan(angle * Mathf.Deg2Rad);
                Debug.Log("angle: " + angle);
                
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
                ballHeldBy = null;
                
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
            
            isBeingHeld = false;
            if (ballHeldBy != null) ballHeldBy.heldBall = false;
            _rigidBody.isKinematic = false;
            //_dribbleTransform = null;
            ballHeldBy = null;
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
                float leftDribblePointX = _dribbleTransform.position.x + (_dribbleTransform.parent.parent.position.x - _dribbleTransform.position.x);

                float modifierX = Mathf.Lerp(_dribbleTransform.position.x, leftDribblePointX, 0.5f + (Mathf.Sin(Time.time * _dribbleTime) / 2));
                

                if (ballHeldBy.navMeshAgent.speed > 10f)
                {
                    if(modifierX > _dribbleTransform.position.x - 0.1f || modifierX == _dribbleTransform.position.x)
                        modifierX = _dribbleTransform.position.x;
                }
                Vector3 landingSpot = new Vector3(modifierX,
                    _floorPosition.y + _sphereCollider.radius / 2, _dribbleTransform.position.z);
                _rigidBody.position = landingSpot + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * _dribbleTime));

            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "BallReset")
            {
                transform.SetPositionAndRotation(originalPos, originalRot);
                _rigidBody.velocity = Vector3.zero;
            }

            if (collision.collider.tag == "Hoop")
            {
                _rigidBody.isKinematic = false;
            }

            if (collision.collider.tag != "Floor") return;
            if (isBeingHeld && ballHeldBy != null)
            {
                IsHittingFloor = true;
                
            }


        }

        private void OnCollisionStay(Collision collision)
        {
            //Test to see if the ball is stuck
            string[] stuckTags = { "HoopOne", "HoopTwo" };
            if(stuckTags.Any(x=> x == collision.collider.tag))
            {
                currentStuckTime += Time.deltaTime;
            }
        }

        void OnCollisionExit(Collision other)
        {
            if (other.collider.tag != "Floor") return;
            if (isBeingHeld && ballHeldBy != null)
            {
                IsHittingFloor = false;
            }
        }
    }
}
