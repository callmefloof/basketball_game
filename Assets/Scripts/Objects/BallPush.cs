using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class BallPush : MonoBehaviour
    {
        public Rigidbody body;

        public void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Ball")
            {
                var force = transform.position - other.transform.position;
                var magnitude = 100f;
                force.Normalize();
                other.GetComponent<Rigidbody>().AddForce(-force * magnitude);



            }
        }

    }
}
