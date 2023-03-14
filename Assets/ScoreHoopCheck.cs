using Assets.Scripts.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHoopCheck : MonoBehaviour
{
    [field: SerializeField] public Hoop Parent { get; private set; }
    [field: SerializeField] public bool isUnder { get; private set; }



    // Start is called before the first frame update
    void Start()
    {
        if (Parent == null) Parent = transform.parent.GetComponent<Hoop>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Parent.Ball.gameObject)
        {
            Parent.SetHitBool(isUnder);
        }
    }
}
