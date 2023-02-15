using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.AI.State_Machine
{
    public interface IStateMachineMember
    {
        public StateMachine StateMachine { get; }
    }
}
