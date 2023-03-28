using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using Assets.Scripts.AI.State_Machine.States.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.AI.State_Machine.States
{
    public class ReceivePass : State
    {
        private Baller baller;

        public ReceivePass(IStateMachineMember owner) : base(owner)
        {
            baller = owner as Baller;
        }

        public override void Enter()
        {
            
        }

        public override void Execute()
        {
            baller.navMeshAgent.destination = baller.receivedPassDestination;

            baller.StateMachine.ChangeState(new Examine(baller));
        }

        public override void Exit()
        {
            
        }
    }
}
