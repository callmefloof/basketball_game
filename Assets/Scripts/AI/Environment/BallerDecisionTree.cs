using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.AI.Environment;
using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using Assets.Scripts.AI.State_Machine.States;
using Assets.Scripts.Objects;
using UnityEngine;

public class BallerDecisionTree : EnvironmentInfoComponent

{

    private Baller owner;
    private EnvironmentInfoComponent environment;
    public  GameObject ownHoop = GameObject.FindWithTag("HoopTwo");
    public GameObject enemyHoop = GameObject.FindWithTag("HoopOne");


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

    private bool ShouldDefend()
    {
        
        if (Vector3.Distance(owner.transform.position, environment.Ball.transform.position) < 1f && 
            Vector3.Distance(environment.Ball.transform.position, ownHoop.transform.position) < 2f)
        {
            return true;
        }

        foreach (var opponent in environment.EnemyTeam)
        {
            if (Vector3.Distance(opponent.transform.position, environment.Ball.transform.position) < 1f)
            {
                return true;
            }
        }
        
        return false;
    }
    private bool ShouldAttack()
    {
        if (Vector3.Distance(environment.Ball.transform.position, enemyHoop.transform.position) < 
            Vector3.Distance(environment.Ball.transform.position, ownHoop.transform.position))
        {
            return true;
        }

        return false;
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
