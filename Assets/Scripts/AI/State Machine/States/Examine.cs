using Assets.Scripts.AI.Environment;
using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using Assets.Scripts.AI.State_Machine.States.Base;
using UnityEngine.SubsystemsImplementation;

namespace Assets.Scripts.AI.State_Machine.States
{
    public class Examine : State
    {

        // Use this for initialization
        public Baller baller;

        public Examine(IStateMachineMember owner) : base(owner)
        {
            baller = owner as Baller;
        }

        public override void Enter()
        {
            
        }

        public override void Execute()
        {
            
            switch (baller.environmentInfoComponent.BallerSuggestion) 
            {
                case BallerInfo.ShouldAttack: 
                    
                        baller.StateMachine.ChangeState(new AttackingState(baller));
                    break; 
                
                case BallerInfo.ShouldShoot: 
                    baller.StateMachine.ChangeState(new ShootingState(baller));
                    break;
                
                case BallerInfo.ShouldDefend: 
                    baller.StateMachine.ChangeState(new defendingState(baller));
                    break;
                //get closer to hoop, then shoot... 
                case BallerInfo.ShouldGetCloserToTeamHoop: 
                    baller.StateMachine.ChangeState(new ShouldGetCloseToHoop(baller, false));
                    break;
                
                case BallerInfo.ShouldGetCloserToEnemyHoop: 
                    baller.StateMachine.ChangeState(new ShouldGetCloseToHoop(baller, true));
                    break;
                //add pass functions 
            }
        }
        //Don't use with examine State
        public override void Exit()
        {
            if (this.GetType() == typeof(Examine))
            {
                return;
            }
        }
    }
}