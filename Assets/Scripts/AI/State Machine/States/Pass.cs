using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using Assets.Scripts.AI.State_Machine.States.Base;
using System;
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



        public override void Execute()
        {
            
            

            List<Vector3> enemyPositions = new List<Vector3>();

            baller.environmentInfoComponent.EnemyTeam.ForEach(x => enemyPositions.Add(x.transform.position));
            Vector3 averagedEnemyPositions = new Vector3(enemyPositions.Sum(x => x.x), enemyPositions.Sum(x => x.y), enemyPositions.Sum(x => x.z));
            Baller nearestTeamBaller = GetFurthestPlayer(averagedEnemyPositions);

            baller.StateMachine.ChangeState(new ShootingState(baller, nearestTeamBaller.transform.position));
        }

        public override void Exit()
        {
            
        }
    }
}
