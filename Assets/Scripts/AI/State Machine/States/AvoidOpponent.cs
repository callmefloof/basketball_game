﻿using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using Assets.Scripts.AI.State_Machine.States.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI.State_Machine.States
{
    public class AvoidOpponent : State
    {
        private Baller baller;
        public AvoidOpponent(IStateMachineMember owner) : base(owner)
        {
            baller = owner as Baller;
        }

        public override void Enter()
        {
            
        }

        public override void Execute()
        {
            if(baller.environmentInfoComponent.EnemyTeam.Any(x=> Vector3.Distance(baller.transform.position, x.transform.position) < 10f * baller.environmentInfoComponent.Defensiveness))
            {
                Vector3 newPosition = baller.transform.position;
                float rng = UnityEngine.Random.Range(0f, 1f);
                if(rng > 0.0f && rng < 0.33f)
                {
                    newPosition = newPosition + baller.environmentInfoComponent.EnemyTeam.Find(x => Vector3.Distance(baller.transform.position, x.transform.position) < 10f * baller.environmentInfoComponent.Defensiveness).transform.right
                    * 10f * baller.environmentInfoComponent.Defensiveness;
                }
                else if ( rng > 0.33f && rng < 0.66f)
                {
                    newPosition = newPosition + -baller.environmentInfoComponent.EnemyTeam.Find(x => Vector3.Distance(baller.transform.position, x.transform.position) < 10f * baller.environmentInfoComponent.Defensiveness).transform.right
                    * 10f * baller.environmentInfoComponent.Defensiveness;
                }
                else
                {
                    newPosition = new Vector3(baller.environmentInfoComponent.EnemyHoop.transform.position.x, baller.transform.position.y, baller.environmentInfoComponent.EnemyHoop.transform.position.z);
                }
                

                baller.navMeshAgent.destination = newPosition;
            }

            baller.StateMachine.ChangeState(new Examine(baller));


        }

        public override void Exit()
        {
            
        }
    }
}
