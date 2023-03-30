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

    /*private void GetDistances()
    {
        
    }*/
    
    private bool ShouldDefend()
    {
        
        foreach (var enemy in environment.EnemyTeam)
        {

            string zoneTagTeam = Owner.team == 1 ? "ZoneOne" : "ZoneTwo";
            var defendingZone = GameObject.FindWithTag(zoneTagTeam).GetComponent<ShootingZone>();
            
            if (!owner.heldBall && enemy.heldBall && !teamHeldBall())
            {
                if (defendingZone)
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
        
        return (ballIsClose && !owner.heldBall);
        
    }
    private bool ShouldShoot()
    {
        if (owner.heldBall  && IsCloseToEnemyHoop())
        {
            return true;
        }

        return false;
    }
    private bool ShouldGetCloserToEnemyHoop()
    {
        if (owner.heldBall)
        {
            if (!IsCloseToEnemyHoop())
            {
                return true;
            }
        }

        else
        {
            if (teamHeldBall())
            {
                if (!IsCloseToEnemyHoop())
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool ShouldGetCloserToTeamHoop()
    {
        /*if (!owner.heldBall && Vector3.Distance(owner.transform.position, ownHoop.transform.position) > 20f)
        {
            return true;
        }*/
        return false;
    }
    private bool AvoidOpponent()
    {
        /*foreach (var opponent in environment.EnemyTeam)
        {
            if (Vector3.Distance(opponent.transform.position, owner.transform.position) < 1f)
            {
                return true;
            }
        }*/
        return false;
    }
    private bool PassBall()
    {
        /*if (owner.heldBall)
        {
            foreach (var teammate in environment.Team)
            {
                if (Vector3.Distance(owner.transform.position, teammate.transform.position) < 10f)
                {
                    return true;
                }
            }
        }*/
        return false;
    }
    
    /*private bool HasFreeTeammate()
    {
        // Check if there is a free teammate to pass to
        // Return true if there is, false otherwise
        return (IsEnemyNearbyTeammate());

    }*/
    
    private bool IsEnemyNearbyTeammate()
    {
        // Check if there is a free teammate to pass to
        // Return true if there is, false otherwise
        foreach (var enemy in EnemyTeam)
        {
            foreach (var teammate in Team)
            {
                if(teammate == owner){continue;}
                float distance = Vector3.Distance(enemy.transform.position, teammate.transform.position);
                if (distance < 5f) // adjust the distance threshold as needed
                {
                    return true;
                }
                
            }
            
        }

        return false;
    }

    private bool teamHeldBall()
    {
        bool teammateHasBall = !environment.Team.Where(teammate => teammate != owner).All(teammate => !teammate.heldBall);
        
        foreach (var enemy in environment.EnemyTeam)
        {
            if (!owner.heldBall && !enemy.heldBall && teammateHasBall)
            {
                return true;
            }
        }
        return false;
    }
    
    private bool IsCloseToEnemyHoop()
    {
        // Check if the player is close to the enemy hoop
        // Return true if they are, false otherwise
        GameObject enemyHoop = GameObject.FindGameObjectWithTag("HoopOne");
        float distanceToHoop = Vector3.Distance(owner.transform.position, enemyHoop.transform.position);
        return distanceToHoop < 8f;
    }
    
}
