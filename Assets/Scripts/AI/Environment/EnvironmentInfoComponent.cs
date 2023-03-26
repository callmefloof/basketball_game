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


namespace Assets.Scripts.AI.Environment
{
    public enum BallerInfo {ShouldDefend, ShouldAttack, ShouldShoot, ShouldGetCloserToEnemyHoop, ShouldGetCloserToTeamHoop, AvoidOpponent}
    public class EnvironmentInfoComponent
    {
        public Baller Owner { get; private set; }
        public List<Baller> Team { get; private set; } = new List<Baller>();
        public List<Baller> EnemyTeam { get; private set; } = new List<Baller>();
        public Hoop OurHoop { get; private set; }
        public Hoop EnemyHoop { get; private set; }
        public Ball Ball { get; private set; }
        private float _aggression = 0.5f;
        public float Aggression
        {
            get => _aggression;
            set => _aggression = value > 1 ? 1 : value < 0 ? 0 : value;
        }

        private float _defensiveness = 0.5f;
        public float Defensiveness
        {
            get => _defensiveness;
            set => _defensiveness = value > 1 ? 1 : value < 0 ? 0 : value;
        }

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
        private (float enemyHoopDistance, float ownerHoopDistance) GetHoopDistances()
        {
            //set y to 0 to return a "2D" result.
            var ownerPosFlattened = new Vector3(Owner.transform.position.x, 0, Owner.transform.position.y);
            var enemyHoopPosFlattened = new Vector3(EnemyHoop.transform.position.x, 0, EnemyHoop.transform.position.z);
            var ownerHoopPosFlattened = new Vector3(OurHoop.transform.position.x, 0, OurHoop.transform.position.z);

            return (Vector3.Distance(ownerPosFlattened, enemyHoopPosFlattened), Vector3.Distance(ownerPosFlattened, ownerHoopPosFlattened));
        }
        private (float enemyHoopDistance, float ownerHoopDistance) GetHoopDistances(Baller baller)
        {
            //set y to 0 to return a "2D" result.
            var ownerPosFlattened = new Vector3(baller.transform.position.x, 0, baller.transform.position.y);
            var enemyHoopPosFlattened = new Vector3(baller.environmentInfoComponent.EnemyHoop.transform.position.x, 0, baller.environmentInfoComponent.EnemyHoop.transform.position.z);
            var ownerHoopPosFlattened = new Vector3(baller.environmentInfoComponent.OurHoop.transform.position.x, 0, baller.environmentInfoComponent.OurHoop.transform.position.z);

            return (Vector3.Distance(ownerPosFlattened, enemyHoopPosFlattened), Vector3.Distance(ownerPosFlattened, ownerHoopPosFlattened));
        }
        private (List<float> enemyDistances, List<float> teamDistances) GetDistances()
        {
            List<float> enemyBallerDistances = EnemyTeam.Select(enemyBaller => Vector3.Distance(Owner.transform.position, enemyBaller.transform.position)).ToList();

            List<float> ourTeamMembersDistances = (from teamBaller in Team where teamBaller != Owner select Vector3.Distance(Owner.transform.position, teamBaller.transform.position)).ToList();
            return (enemyBallerDistances, ourTeamMembersDistances);
        }
        private (List<float> enemyDistances, List<float> teamDistances) GetDistances(Baller baller)
        {
            List<float> enemyBallerDistances = baller.environmentInfoComponent.EnemyTeam.Select(enemyBaller => Vector3.Distance(baller.transform.position, enemyBaller.transform.position)).ToList();
            List<float> ourTeamMembersDistances = (from teamBaller in baller.environmentInfoComponent.Team where teamBaller != Owner select Vector3.Distance(baller.transform.position, teamBaller.transform.position)).ToList();
            return (enemyBallerDistances, ourTeamMembersDistances);
        }
        private void SetAvoidance()
        {
            Owner.navMeshAgent.avoidancePriority = Mathf.FloorToInt(Mathf.Lerp(minAvoidancePriority, maxAvoidanceDistance, Aggression));
            Owner.navMeshAgent.radius = Mathf.FloorToInt(Mathf.Lerp(minAvoidanceDistance, maxAvoidanceDistance, Defensiveness));
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
                    return distanceDifference > Aggression;
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
                
                float chance = (1f-(distanceFromPlayerToHoop / distanceFromCentreToHoop) +(Aggression-Defensiveness));

                return chance > UnityEngine.Random.Range(0f, 1f);
            }
            //
        }
        public void UpdateInfo()
        {
            SetAvoidance();
            var ownerPosition = Owner.transform.position;
            BallerSuggestion = Ball.isBeingHeld switch
            {
                // TODO: determine what part of the field the baller is on.
                // check to see if we're holding the ball
                true when Ball.ballHeldBy == Owner && Owner.heldBall => ShouldShoot() ? BallerInfo.ShouldShoot : BallerInfo.AvoidOpponent,
                    
                true when Team.Any(x=> Ball.ballHeldBy == x && Ball.ballHeldBy != Owner ) => BallerInfo.ShouldDefend,
                true => ShouldIntercept(ownerPosition)
                    ? BallerInfo.ShouldAttack
                    : BallerInfo.ShouldGetCloserToTeamHoop,
                false => ShouldPickUpBall()
                    ? BallerInfo.ShouldAttack
                    : BallerInfo.ShouldDefend
            };
        }
    }
}
