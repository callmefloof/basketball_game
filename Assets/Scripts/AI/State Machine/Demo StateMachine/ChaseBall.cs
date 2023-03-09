using UnityEngine;

namespace Assets.Scripts.AI.State_Machine.Demo_StateMachine
{
    public class ChaseBall : MonoBehaviour
    {
        [SerializeField] private Vector3 basketballPosition;
        [SerializeField]private Vector3 pos;
        [SerializeField] GameObject Baller;
        private Vector3 newPos;
        public float speed = 10.0f;
        public bool heldBall = false;

        // Start is called before the first frame update
        void Start()
        {
            pos = GameObject.FindWithTag("AI 1").transform.position;

        }

        // Update is called once per frame
        void Update()
        {
            var step =  speed * Time.deltaTime; // calculate distance to move
        
            basketballPosition = GameObject.FindWithTag("Ball").transform.position;
            pos = GameObject.FindWithTag("AI 1").transform.position;
            transform.position = Vector3.MoveTowards(pos, basketballPosition, step);
        }
    
        void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Ball") // Check if the collision object has a specific tag
            {
                heldBall = true;
                Debug.Log("Collision detected with player object!");
                Debug.Log("The ball is held? " + heldBall.ToString());
                // Do something when the collision occurs
            }
        }
    }
}
