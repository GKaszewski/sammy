using UnityEngine;

namespace Sammy.AI
{
    public class ChasingState : LionBaseState
    {
        public ChasingState(LionAI owner) : base(owner) { }

        public override void Execute()
        {
            owner.Agent.SetDestination(owner.target.transform.position);
            
            var distanceFromPlayer = Vector3.Distance(owner.transform.position, owner.target.transform.position);
            if (distanceFromPlayer <= owner.attackRange)
            {
                // owner.StateMachine.ChangeState(new AttackingState(owner));
            }
        }
    }
}