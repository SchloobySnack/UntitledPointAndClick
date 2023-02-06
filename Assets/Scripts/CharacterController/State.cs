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
            return;
        }
        public override void Update()
        {        
            return;
        }
        public override void Exit()
        {
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
            if (CManager.IsFacingTarget(CManager.target))
            {
                CManager.setState(new Moving(CManager));
            }
            else
            {
                CManager.RotateTowardsTarget(CManager.target);
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
            Debug.Log("Entered Moving");
        }
        public override void Update()
        {
            CManager.CharacterMove();
            return;
        }
        public override void Exit()
        {
            Debug.Log("Exited Moving");
            return;
        }

    }
}