using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using Assets.Scripts.AI.State_Machine.States.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI.State_Machine.States
{
    public class Pass : State
    {
        private Baller baller;
        public Pass(IStateMachineMember owner) : base(owner)
        {
            baller = owner as Baller;
        }

        public override void Enter()
        {
            
        }

        private Baller GetFurthestPlayer(Vector3 from)
        {
            Baller bMax = null;
            float maxDist = 0f;
            
            foreach (Baller b in baller.environmentInfoComponent.Team)
            {
                float dist = Vector3.Distance(b.transform.position, from);
                if (dist > maxDist)
                {
                    bMax = b;
                    maxDist = dist;
                }
            }
            return bMax;
        }

        //Stand Still for 1 second
        public IEnumerator Wait()
        {
            float speed = baller.speed;
            baller.speed = 0f;
            yield return new WaitForSeconds(1f);
            baller.speed = speed;
        }


        public override void Execute()
        {
            
            

            List<Vector3> enemyPositions = new List<Vector3>();

            baller.environmentInfoComponent.EnemyTeam.ForEach(x => enemyPositions.Add(x.transform.position));
            Vector3 averagedEnemyPositions = new Vector3(enemyPositions.Average(x => x.x), enemyPositions.Average(x => x.y), enemyPositions.Average(x => x.z));
            Baller nearestTeamBaller = GetFurthestPlayer(averagedEnemyPositions);
            nearestTeamBaller.receivingPass = true;
            var targetDestination = nearestTeamBaller.transform.position + nearestTeamBaller.transform.forward;
            nearestTeamBaller.receivedPassDestination = targetDestination;
            baller.StartCoroutine(Wait());
            baller.StateMachine.ChangeState(new ShootingState(baller, nearestTeamBaller.transform.position));
        }

        public override void Exit()
        {
            
        }
    }
}
