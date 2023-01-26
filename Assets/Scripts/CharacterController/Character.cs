using System;
using UnityEngine;
using UnityEngine.AI;
namespace HeyAlexi
{
    public class Character : MonoBehaviour
    {
        private Animator anim;
        private NavMeshAgent agent;
        private NavMeshPath playerPath;
        Vector2 smoothDeltaPosition = Vector2.zero;
        Vector2 velocity = Vector2.zero;
        public CurrentState currentState;

        void Start()
        {
            anim = GetComponentInChildren<Animator>();
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            // agent.updatePosition = false;

            currentState = new CurrentState(new Idle());
            currentState.State.Enter();

        }

        void Update()
        {
            currentState.State.Update();
            // Vector3 worldDeltaPosition = agent.destination - transform.position;

            // // Map 'worldDeltaPosition' to local space
            // float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            // float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            // Vector2 deltaPosition = new Vector2(dx, dy);

            // // Low-pass filter the deltaMove
            // float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            // smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

            // // Update velocity if delta time is safe
            // if (Time.deltaTime > 1e-5f)
            //     velocity = smoothDeltaPosition / Time.deltaTime;

            // bool shouldMove = velocity.magnitude > 0.01f && agent.remainingDistance > agent.radius;

            // // Update animation parameters
            // anim.SetBool("move", shouldMove);
            // anim.SetFloat("velx", velocity.x);
            // anim.SetFloat("vely", velocity.y);

            // LookAt lookAt = GetComponent<LookAt>();
            // if (lookAt)
            //     lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;

            //		// Pull character towards agent
            //		if (worldDeltaPosition.magnitude > agent.radius)
            //			transform.position = agent.nextPosition - 0.9f*worldDeltaPosition;

            //		// Pull agent towards character
            //		if (worldDeltaPosition.magnitude > agent.radius)
            //			agent.nextPosition = transform.position + 0.9f*worldDeltaPosition;
        }

        void OnAnimatorMove()
        {
            // Update postion to agent position
            //		transform.position = agent.nextPosition;

            // Update position based on animation movement using navigation surface height
            // Vector3 position = anim.rootPosition;
            // position.y = agent.nextPosition.y;
            // transform.position = position;
        }

        // Transform target, Vector3 targetPosition
        public void NavToTarget(RaycastHit hit)
        {
            playerPath ??= agent.path;
            Vector3 targetPosition = hit.point;
            if (currentState.State is Idle )
            {
                agent.CalculatePath(targetPosition, playerPath);
                if (!(agent.path.status == NavMeshPathStatus.PathComplete))
                {
                    agent.ResetPath();
                    return;
                }
                agent.SetPath(playerPath);
            }
        }

        private bool IsInteractable(GameObject gameObject)
        {
            if (gameObject.CompareTag("Interactable"))
            {
                return true;
            }
            return false;
        }

    }

    public abstract class State
    {   
        public State()
        {

        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();

    }

    public class CurrentState
    {
        private State state;

        public CurrentState(State state)
        {
            this.State = state;
        }
        public State State
        {
            get { return state; }
            set { state = value; }
        }

        public void Enter()
        {
            return;
        }
        public void Update()
        {
            return;
        }
        public void Exit()
        {
            return;
        }

    }

    public class Idle : State
    {
        public string name = "Idle";

        public override void Enter()
        {
            
            return;
        }
        public override void Update()
        {
            return;
        }
        public override void Exit()
        {
            return;
        }

    }

    public class Move : State
    {
        public override void Enter()
        {
            Debug.Log("Moving");
        }
        public override void Update()
        {
            return;
        }
        public override void Exit()
        {
            return;
        }

    }

}