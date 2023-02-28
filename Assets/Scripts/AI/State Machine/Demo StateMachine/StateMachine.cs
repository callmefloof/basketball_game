namespace statemachine
{

    /// <summary>
    /// A simple state maching
    /// </summary>
    public class StateMachine
    {
        // the states
        public State attackingState;
        public State shootingState;

        private State currentState;

        public StateMachine(Baller owner)
        {
            attackingState = new AttackingState(owner, this);
            shootingState = new ShootingState(owner, this);
            SwitchState(attackingState);
        }

        /// <summary>
        /// Switch from one state to another
        /// </summary>
        /// <param name="newState">The new state to be</param>
        public void SwitchState(State newState)
        {
            //Debug.Log("Switching state to: " + newState.GetType().Name);
            currentState?.ExitState();
            currentState = newState;
            currentState.EnterState();
        }

        /// <summary>
        /// Update the current state (let it do its thing)
        /// </summary>
        public void UpdateState()
        {
            currentState.UpdateState();
        }
    }
}