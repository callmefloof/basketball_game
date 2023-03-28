using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.AI.Environment;
using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using Assets.Scripts.AI.State_Machine.States;
using Assets.Scripts.Objects;
using UnityEngine;

public class BallerDecisionTree : EnvironmentInfoComponent

{

    private Baller owner;
    private EnvironmentInfoComponent environment;

    public BallerDecisionTree(Baller owner, EnvironmentInfoComponent environment) : base(owner)
    {
        this.owner = owner;
        this.environment = environment;
    }

    public override void UpdateInfo()
    {
        if (this.ShouldDefend())
        {
           
        }
        else if (this.ShouldAttack())
        {
            
        }
        else if (this.ShouldShoot())
        {
            
        }
        else if (this.ShouldGetCloserToEnemyHoop())
        {
            
        }
        else if (this.ShouldGetCloserToTeamHoop())
        {
            
        }
        else if (this.AvoidOpponent())
        {
            
        }
        else if (this.PassBall())
        {
           
        }
        else
        {
            
        }
    }

    private bool ShouldDefend()
    {
        return false;
    }
    private bool ShouldAttack()
    {
        return false;
    }
    private bool ShouldShoot()
    {
        return false;
    }
    private bool ShouldGetCloserToEnemyHoop()
    {
        return false;
    }
    private bool ShouldGetCloserToTeamHoop()
    {
        return false;
    }
    private bool AvoidOpponent()
    {
        return false;
    }
    private bool PassBall()
    {
        return false;
    }
}
