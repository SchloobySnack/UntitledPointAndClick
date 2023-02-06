using System.Collections;
using UnityEngine;

namespace HeyAlexi.Character
{
    public abstract class State
    {
        protected Manager CManager;

        public State(Manager manager)
        {
            CManager = manager;
        }
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }

    public class Idle : State
    {
        public Idle(Manager manager) : base(manager){}
        public override void Enter()
        {
            Debug.Log("Entered Idle");
            CManager.agent.ResetPath();
            CManager.agent.isStopped = true;
            return;
        }
        public override void Update()
        {
            return;
        }
        public override void Exit()
        {
            CManager.agent.isStopped = false;
            Debug.Log("Exited Idle");
            return;
        }

    }

    public class Move : State
    {
        public Move(Manager manager) : base(manager){}
        public override void Enter()
        {
            Debug.Log("Entered Move");
        }
        public override void Update()
        {
            if (CManager.IsFacingPos(CManager.targetPos))
            {
                CManager.setState(new Moving(CManager));
            }
            else
            {
                CManager.RotateTowardsPos(CManager.targetPos);
            }
            return;
        }
        public override void Exit()
        {
            Debug.Log("Exited Move");
            return;
        }

    }
    public class Moving : State
    {
        public Moving(Manager manager) : base(manager) { }
        public override void Enter()
        {
            CManager.NavToTarget(CManager.targetPos);
            Debug.Log("Entered Moving");
        }
        public override void Update()
        {
            CManager.CharacterMove();
            return;
        }
        public override void Exit()
        {
            CManager.CharacterStop();
            Debug.Log("Exited Moving");
            return;
        }

    }
}