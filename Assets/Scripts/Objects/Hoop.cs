using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Hoop : MonoBehaviour
    {
        // Start is called before the first frame update
        public int team = 1;
        private Ball ball;

        void Start()
        {
            ball = FindObjectOfType<Ball>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == ball.gameObject)
            {
                switch (team)
                {
                    case 1:
                        GameManager.Instance.Scores["Team 2"]++;
                        break;
                    case 2:
                        GameManager.Instance.Scores["Team 1"]++;
                        break;

                }
                
            }
        }
    }
}
