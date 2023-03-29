using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Assets.Scripts.Objects;
using Object = UnityEngine.Object;
using UnityEngine.Internal;

namespace Assets.Scripts.AI.Environment
{
    public enum BallerInfo {ShouldDefend, ShouldAttack, ShouldShoot, ShouldGetCloserToEnemyHoop, ShouldGetCloserToTeamHoop, AvoidOpponent, PassBall, ReceivePassedBall}
    
    public class EnvironmentInfoComponent
    {
        public Baller Owner { get; private set; }
        public List<Baller> Team { get; private set; } = new List<Baller>();
        public List<Baller> EnemyTeam { get; private set; } = new List<Baller>();
        public Hoop OurHoop { get; private set; }
        public Hoop EnemyHoop { get; private set; }
        public Ball Ball { get; private set; }
        

        public readonly float maxAvoidanceDistance = 5f;
        public readonly float minAvoidanceDistance = 0.5f;
        /*
         *  The avoidance priority level.
            When the agent is performing avoidance, agents of lower priority are ignored. The valid range is from 0 to 99 where: Most important = 0. Least important = 99. Default = 50.
            NOTE: 0 is too low for it to meaningly move
         */
        public readonly int minAvoidancePriority = 99;
        public readonly int maxAvoidancePriority = 0;

        public BallerInfo BallerSuggestion { get; private set; }
        public EnvironmentInfoComponent(Baller owner)
        {


            Owner = owner;
            var teamresults = GetTeams();
            EnemyTeam.AddRange(teamresults.enemyBallers);
            Team.AddRange(teamresults.teamBallers);
            Ball = Object.FindFirstObjectByType<Ball>();
            EnemyHoop = Object.FindObjectsOfType<Hoop>().First(x => x.team != Owner.team);
            OurHoop = Object.FindObjectsOfType<Hoop>().First(x => x.team == Owner.team);
            SetAvoidance();
        }
        private (List<Baller> enemyBallers, List<Baller> teamBallers) GetTeams()
        {
            var ballers = Object.FindObjectsOfType<Baller>();
            List<Baller> enemyBallers = new List<Baller>();
            List<Baller> teamBallers = new List<Baller>();
            foreach (Baller baller in ballers)
            {
                if (baller.team != Owner.team)
                {
                    enemyBallers.Add(baller);
                }
                else
                {
                    teamBallers.Add(baller);
                }
            }
            return (enemyBallers, teamBallers);
        }
        protected (float enemyHoopDistance, float ownerHoopDistance) GetHoopDistances()
        {
            //set y to 0 to return a "2D" result.
            var ownerPosFlattened = new Vector3(Owner.transform.position.x, 0, Owner.transform.position.y);
            var enemyHoopPosFlattened = new Vector3(EnemyHoop.transform.position.x, 0, EnemyHoop.transform.position.z);
            var ownerHoopPosFlattened = new Vector3(OurHoop.transform.position.x, 0, OurHoop.transform.position.z);

            return (Vector3.Distance(ownerPosFlattened, enemyHoopPosFlattened), Vector3.Distance(ownerPosFlattened, ownerHoopPosFlattened));
        }
        protected (float enemyHoopDistance, float ownerHoopDistance) GetHoopDistances(Baller baller)
        {
            //set y to 0 to return a "2D" result.
            var ownerPosFlattened = new Vector3(baller.transform.position.x, 0, baller.transform.position.y);
            var enemyHoopPosFlattened = new Vector3(baller.environmentInfoComponent.EnemyHoop.transform.position.x, 0, baller.environmentInfoComponent.EnemyHoop.transform.position.z);
            var ownerHoopPosFlattened = new Vector3(baller.environmentInfoComponent.OurHoop.transform.position.x, 0, baller.environmentInfoComponent.OurHoop.transform.position.z);

            return (Vector3.Distance(ownerPosFlattened, enemyHoopPosFlattened), Vector3.Distance(ownerPosFlattened, ownerHoopPosFlattened));
        }
        protected (List<float> enemyDistances, List<float> teamDistances) GetDistances()
        {
            List<float> enemyBallerDistances = EnemyTeam.Select(enemyBaller => Vector3.Distance(Owner.transform.position, enemyBaller.transform.position)).ToList();

            List<float> ourTeamMembersDistances = (from teamBaller in Team where teamBaller != Owner select Vector3.Distance(Owner.transform.position, teamBaller.transform.position)).ToList();
            return (enemyBallerDistances, ourTeamMembersDistances);
        }
        protected (List<float> enemyDistances, List<float> teamDistances) GetDistances(Baller baller)
        {
            List<float> enemyBallerDistances = baller.environmentInfoComponent.EnemyTeam.Select(enemyBaller => Vector3.Distance(baller.transform.position, enemyBaller.transform.position)).ToList();
            List<float> ourTeamMembersDistances = (from teamBaller in baller.environmentInfoComponent.Team where teamBaller != Owner select Vector3.Distance(baller.transform.position, teamBaller.transform.position)).ToList();
            return (enemyBallerDistances, ourTeamMembersDistances);
        }
        protected void SetAvoidance()
        {
            //Owner.navMeshAgent.avoidancePriority = Mathf.FloorToInt(Mathf.Lerp(minAvoidancePriority, maxAvoidanceDistance, Owner.Aggression));
            Owner.navMeshAgent.radius = Mathf.FloorToInt(Mathf.Lerp(minAvoidanceDistance, maxAvoidanceDistance, Owner.Defensiveness));
            //This was changed due to obstacle avoidance component is removed (it produced some unexpected behavior)
            //Turns out that only avoidancepriority is needed to make NavMeshAgents avoid one another.
            Owner.navMeshAgent.avoidancePriority = 10;
            
        }
        private bool ShouldPickUpBall()
        {
            return Team.All(x => x.heldBall != true);
            //for each, for each player check if they(our team mate) arent holding the ball and return the values , if not attack, if then get closer or shoot (other team defend)
        }
        private bool ShouldIntercept(Vector3 ownerPosition)
        {
            foreach (var opponent in EnemyTeam)
            {
                //continue if the opponent isn't holding the ball.
                if (!opponent.heldBall) continue;
                foreach(var teamMember in Team)
                {
                    // if the team member is ourselves, continue.
                    if (teamMember == Owner) continue;

                    //get the difference back as: distance owner to opponent / distance other team member to opponent
                    float distanceDifference = Vector3.Distance(opponent.transform.position, ownerPosition) / Vector3.Distance(teamMember.transform.position, ownerPosition);

                    // example: if the distances of both is 5, the result is 1
                    // when our owner's agression is 1, it will pursue the ball, if not, it will get closer to the team hoop
                    return distanceDifference > Owner.Aggression;
                } 
            }

            //fallback option
            return true;
        }

        private bool ShouldShoot()
        {
            //We need to get near the opponent's side first
            if (!Owner.shoot) return false;
            else
            {
                Vector3 enemyHoopPosXZ = new Vector3(EnemyHoop.transform.position.x, Owner.transform.position.y, EnemyHoop.transform.position.z);
                var centerPoint = GameObject.FindGameObjectWithTag("CenterPoint");
                Vector3 centerPointPosXZ = new Vector3(centerPoint.transform.position.x, Owner.transform.position.y, centerPoint.transform.position.z);
                float distanceFromCentreToHoop = Vector3.Distance(centerPointPosXZ, enemyHoopPosXZ);
                float distanceFromPlayerToHoop = Vector3.Distance(Owner.transform.position, enemyHoopPosXZ);
                //prevents repeated shooting when it would only shoot from below the hoop
                if (distanceFromPlayerToHoop < 3f) return false;

                float chance = (1f-(distanceFromPlayerToHoop / distanceFromCentreToHoop) +(Owner.Aggression - Owner.Defensiveness));

                return chance > UnityEngine.Random.Range(0f, 1f);
            }
            //
        }
        private Baller GetClosestEnemy()
        {
            Baller bMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = Owner.transform.position;
            foreach (Baller b in EnemyTeam)
            {
                float dist = Vector3.Distance(b.transform.position, currentPos);
                if (dist < minDist)
                {
                    bMin = b;
                    minDist = dist;
                }
            }
            return bMin;
        }

        private Baller[] GetFurthestPlayer(Vector3 from)
        {
            Baller[] bRanking = Team.Where(x => x != Owner).OrderBy(x=> Vector3.Distance(x.transform.position, from)).ToArray();
            //float maxDist = 0f;

            //foreach (Baller b in Team)
            //{
            //    if (b == Owner) continue;
            //    float dist = Vector3.Distance(b.transform.position, from);
            //    if (dist > maxDist)
            //    {
            //        bMax = b;
            //        maxDist = dist;
            //    }
            //}
            return bRanking;
        }

        private bool ShouldPass()
        {
            float rng = UnityEngine.Random.Range(0.0f, 1.0f);
            float minDistanceEnemy = 1f;
            float maxDistanceEnemy = 5f;
            float defensivenessFactor = 1.2f;
            float agressionFactor = 0.8f;

            var nearestEnemy = GetClosestEnemy();
            
            if(nearestEnemy == null) return false;
            
            float distance = Vector3.Distance(Owner.transform.position, nearestEnemy.transform.position);
            
            float distanceClamped = Mathf.Clamp(distance, minDistanceEnemy, maxDistanceEnemy);
            
            
            //1 if distance is 1, 0 if distance is >5
            
            float distanceFactor = Mathf.Abs(1f-(distanceClamped - minDistanceEnemy/ (maxDistanceEnemy - minDistanceEnemy)));
            

            float chance = distanceFactor * (Owner.Defensiveness*defensivenessFactor - Owner.Aggression*agressionFactor);

            //if we are too close to another player, don't bother trying to pass
            var furthestPlayers = GetFurthestPlayer(Owner.transform.position);
            if(furthestPlayers == null) return false;
            string zoneTagTeam = Owner.team == 1 ? "ZoneOne" : "ZoneTwo";
            var shootingZoneFriendly = GameObject.FindGameObjectWithTag(zoneTagTeam).GetComponent<ShootingZone>();
            if (shootingZoneFriendly == null) return false;
            if (furthestPlayers.All(x => shootingZoneFriendly.BallersInZone.Contains(x) && !shootingZoneFriendly.BallersInZone.Contains(Owner))) return false;
            if (furthestPlayers.All(x=> Vector3.Distance(Owner.transform.position, x.transform.position) < maxDistanceEnemy)) return false;

            return chance > rng;


        }
        public virtual void UpdateInfo()
        {
            
            var ownerPosition = Owner.transform.position;
            BallerSuggestion = Ball.isBeingHeld switch
            {
                // TODO: determine what part of the field the baller is on.
                // check to see if we're holding the ball
                true when Ball.ballHeldBy == Owner && Owner.heldBall => ShouldShoot() ? BallerInfo.ShouldShoot : ShouldPass() ? BallerInfo.PassBall: BallerInfo.AvoidOpponent,
                    
                true when Team.Any(x=> Ball.ballHeldBy == x && Ball.ballHeldBy != Owner ) => BallerInfo.ShouldGetCloserToEnemyHoop,
                true => ShouldIntercept(ownerPosition)
                    ? BallerInfo.ShouldAttack
                    : BallerInfo.ShouldDefend,
                false when Owner.receivingPass => BallerInfo.ReceivePassedBall,
                false => ShouldPickUpBall()
                    ? BallerInfo.ShouldAttack
                    : BallerInfo.ShouldDefend
                
            };
        }
    }
}
