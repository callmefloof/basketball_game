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
        [SerializeField] private bool hitbottom = false;
        [SerializeField] private bool hittop = false;
        [field: SerializeField] public Ball Ball { get; private set; }
        [SerializeField] private bool canBeValid = false;
        [SerializeField] private float maxTestTime = 2.5f;
        [SerializeField] private float curTestTime = 0f;

        void Start()
        {
            Ball = FindObjectOfType<Ball>();
        }

        // Update is called once per frame
        void Update()
        {
            if (hittop)
            {
                curTestTime += Time.deltaTime;
                if(curTestTime > maxTestTime)
                {
                    hittop = false;
                    hitbottom = false;
                }
            }
            else
            {
                curTestTime = 0f;
            }
        }

        public void SetHitBool(bool isUnder)
        {
            if (isUnder)
            {
                hitbottom = true;
            }
            else
            {
                hittop = true;
            }
            
            RunScoreCheck();
        }

        public void RunScoreCheck()
        {
            if(hittop && !hitbottom)
            {
                canBeValid = true;
                return;
            }
            if(hitbottom && !hittop)
            {
                canBeValid=false;
                return;
            }

            if(canBeValid && hittop && hitbottom)
            {
                switch (team)
                {
                    case 1:
                        GameManager.Instance.Scores["Team 2"]++;
                        Debug.Log("Team 1 scored");
                        break;
                    case 2:
                        GameManager.Instance.Scores["Team 1"]++;
                        Debug.Log("Team 2 scored");
                        break;

                }
                canBeValid = false;
                hitbottom = false;
                hittop = false;


            }
            
        }
    }
}
