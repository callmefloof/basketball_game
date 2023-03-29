using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Objects
{
    public class BallPush : MonoBehaviour
    {
        private NavMeshObstacle obstacle;
        public float Magnitude = 100f;
        public float Rate = 10f;
        private float defaultSizeX;
        private float defaultSizeZ;
        public float minSizeX = 1f;
        public float minSizeZ = 1f;
        public bool Expanding = false;

        private void Start()
        {
            obstacle = GetComponent<NavMeshObstacle>();
            defaultSizeX = obstacle.size.x;
            defaultSizeZ = obstacle.size.z;
            minSizeZ = minSizeZ * (defaultSizeX / defaultSizeZ);
            obstacle.size = new Vector3(minSizeX, 0f, minSizeZ);
            StartCoroutine(Expand());
            StartCoroutine(Contract());

        }


        public IEnumerator Expand()
        {
            while (true)
            {
                if (Expanding)
                {
                    if (obstacle.size.x < defaultSizeX) obstacle.size = obstacle.size + (new Vector3(0.1f, 0f, 0f) * Time.fixedDeltaTime);
                    if (obstacle.size.z < defaultSizeZ) obstacle.size = obstacle.size + (new Vector3(0f, 0, 0.1f) * Time.fixedDeltaTime);

                }
                yield return new WaitForFixedUpdate();
            }
            
        }

        public IEnumerator Contract()
        {
            while (true)
            {
                if (!Expanding)
                {
                    if (obstacle.size.x > minSizeX) obstacle.size = obstacle.size + (new Vector3(-0.1f, 0f, 0f) * Time.fixedDeltaTime);
                    if (obstacle.size.z > minSizeZ) obstacle.size = obstacle.size + (new Vector3(0f, 0, -0.1f) * Time.fixedDeltaTime);
                }
                yield return new WaitForFixedUpdate();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Ball") Expanding = true;
        }

        public void OnTriggerStay(Collider other)
        {
            if(other.tag == "Ball")
            {
                var posXZ = new Vector3(transform.position.x, 0f, transform.position.z);
                var ballPosXZ = new Vector3(other.transform.position.x, 0f, other.transform.position.z);
                var force = posXZ - ballPosXZ;
               
                force.Normalize();
                other.GetComponent<Rigidbody>().AddForce(-force * Magnitude*Time.fixedDeltaTime);
                
            }
        }


        public void OnTriggerExit(Collider other)
        {
            if (other.tag == "Ball") Expanding = false;
        }

    }
}
