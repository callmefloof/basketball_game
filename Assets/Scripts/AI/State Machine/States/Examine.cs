using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using Assets.Scripts.AI.State_Machine.States.Base;

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
            if (!baller.heldBall)
            {
                baller.StateMachine.ChangeState(new AttackingState(baller));
            }
            else 
            {
                baller.StateMachine.ChangeState(new ShootingState(baller));
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