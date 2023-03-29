using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using System.Linq;
using Assets.Scripts.AI.Environment;
using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using Assets.Scripts.AI.State_Machine.States;
using Assets.Scripts.Objects;
using Unity.VisualScripting;
using UnityEngine;

public class BallerDecisionTree : EnvironmentInfoComponent

{

    private Baller owner;
    private EnvironmentInfoComponent environment;
    public  GameObject ownHoop = GameObject.FindWithTag("HoopTwo");
    public GameObject enemyHoop = GameObject.FindWithTag("HoopOne");
    public bool ballIsClose;


    public BallerDecisionTree(Baller owner) : base(owner)
    {
        this.owner = owner;
        this.environment = new EnvironmentInfoComponent(owner);
        
        
    }

    public override void UpdateInfo()
    {
        if (this.ShouldDefend())
        {
            BallerSuggestion = BallerInfo.ShouldDefend;
        }
        else if (this.ShouldAttack())
        {
            BallerSuggestion = BallerInfo.ShouldAttack;
        }
        else if (this.ShouldShoot())
        {
            BallerSuggestion = BallerInfo.ShouldShoot;
        }
        else if (this.ShouldGetCloserToEnemyHoop())
        {
            BallerSuggestion = BallerInfo.ShouldGetCloserToEnemyHoop;
        }
        else if (this.ShouldGetCloserToTeamHoop())
        {
            BallerSuggestion = BallerInfo.ShouldGetCloserToTeamHoop;
        }
        else if (this.AvoidOpponent())
        {
            BallerSuggestion = BallerInfo.AvoidOpponent;
        }
        else if (this.PassBall())
        {
            BallerSuggestion = BallerInfo.PassBall;
        }
            
    }

    private void GetDistances()
    {
        
    }
    private bool ShouldDefend()
    {
       bool teammateHasBall = !environment.Team.All(teammate => teammate.heldBall == false);
        
        foreach (var enemy in environment.EnemyTeam)
        {

            string zoneTagTeam = Owner.team == 1 ? "ZoneOne" : "ZoneTwo";
           var shootingZoneFriendly = GameObject.FindWithTag(zoneTagTeam).GetComponent<ShootingZone>();
            
            if (!owner.heldBall && enemy.heldBall && !teammateHasBall)
            {
                if (shootingZoneFriendly)
                {
                    
                    return true;
                }
            }
        }
        
        return false;
    }
    private bool ShouldAttack()
    {
        ballIsClose = Vector3.Distance(owner.transform.position, environment.Ball.transform.position) < 3f;
        bool teammateHasBall = !environment.Team.All(teammate => teammate.heldBall == false);
        bool closeToEnemyBasket = Vector3.Distance(owner.transform.position, environment.EnemyHoop.transform.position) < 10f;

        return (ballIsClose || teammateHasBall || closeToEnemyBasket);
        
        
    }
    private bool ShouldShoot()
    {
        if (Vector3.Distance(owner.transform.position, enemyHoop.transform.position) < 10f &&
            Vector3.Distance(owner.transform.position, environment.Ball.transform.position) < 2f)
        {
            return true;
        }
        return false;
    }
    private bool ShouldGetCloserToEnemyHoop()
    {
        if (Vector3.Distance(owner.transform.position, enemyHoop.transform.position) > 20f)
        {
            return true;
        }
        return false;
    }
    private bool ShouldGetCloserToTeamHoop()
    {
        if (!owner.heldBall && Vector3.Distance(owner.transform.position, ownHoop.transform.position) > 20f)
        {
            return true;
        }
        return false;
    }
    private bool AvoidOpponent()
    {
        foreach (var opponent in environment.EnemyTeam)
        {
            if (Vector3.Distance(opponent.transform.position, owner.transform.position) < 1f)
            {
                return true;
            }
        }
        return false;
    }
    private bool PassBall()
    {
        if (owner.heldBall)
        {
            foreach (var teammate in environment.Team)
            {
                if (Vector3.Distance(owner.transform.position, teammate.transform.position) < 10f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
