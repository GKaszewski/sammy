namespace Sammy.AI
{
    public class StateMachine
    {
        public LionBaseState CurrentState { get; private set; }

        public void Initialize(LionBaseState startingState)
        {
            CurrentState = startingState;
            startingState.Enter();
        }
        
        public void ChangeState(LionBaseState newState)
        {
            CurrentState?.Exit();

            CurrentState = newState;
            newState.Enter();
        }
    }
}