namespace Sammy.AI
{
    public class LionBaseState
    {
        protected LionAI owner;
        public LionBaseState(LionAI owner) { this.owner = owner; }
        
        public virtual void Enter() { }
        public virtual void Execute() { }
        public virtual void Exit() { }
    }
}