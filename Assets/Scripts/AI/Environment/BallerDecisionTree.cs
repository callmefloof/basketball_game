using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.AI.Environment;
using Assets.Scripts.AI.State_Machine.States;
using UnityEngine;

public class BallerDecisionTree : EnvironmentInfoComponent

{

    private Baller owner;
    private EnvironmentInfoComponent environment;

    public BallerDecisionTree(Baller owner, EnvironmentInfoComponent environment)
    {
        this.owner = owner;
        this.environment = environment;
    }

    public override  BallerInfo UpdateInfo()
    {
        if (ShouldDefend())
        {
            return BallerInfo.ShouldDefend;
        }
        else if (ShouldAttack())
        {
            return BallerInfo.ShouldAttack;
        }
        else if (ShouldShoot())
        {
            return BallerInfo.ShouldShoot;
        }
        else if (ShouldGetCloserToEnemyHoop())
        {
            return BallerInfo.ShouldGetCloserToEnemyHoop;
        }
        else if (ShouldGetCloserToTeamHoop())
        {
            return BallerInfo.ShouldGetCloserToTeamHoop;
        }
        else if (AvoidOpponent())
        {
            return BallerInfo.AvoidOpponent;
        }
        else if (PassBall())
        {
            return BallerInfo.PassBall;
        }
        else
        {
            return BallerInfo.ShouldGetCloserToEnemyHoop;
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
