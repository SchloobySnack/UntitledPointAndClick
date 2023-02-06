using System;
using UnityEngine;
using UnityEngine.AI;
namespace HeyAlexi.Character
{
    public class Manager : MonoBehaviour
    {
        private Animator anim;
        private NavMeshAgent agent;
        private NavMeshPath playerPath;
        Vector2 smoothDeltaPosition = Vector2.zero;
        Vector2 velocity = Vector2.zero;
        public State currentState;
        public Transform target;

        void Start()
        {
            anim = GetComponentInChildren<Animator>();
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            // agent.updatePosition = false;
            currentState = new Idle(this);
            currentState.Enter();
        }

        void Update()
        {
            currentState.Update();
        }

        public void CharacterMove()
        {
            Vector3 worldDeltaPosition = agent.destination - transform.position;

            // Map 'worldDeltaPosition' to local space
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            // Low-pass filter the deltaMove
            float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

            // Update velocity if delta time is safe
            if (Time.deltaTime > 1e-5f)
                velocity = smoothDeltaPosition / Time.deltaTime;

            bool Moving = velocity.magnitude > 0.01f && agent.remainingDistance > agent.radius;

            // Update animation parameters
            anim.SetBool("move", Moving);
            anim.SetFloat("velx", velocity.x);
            anim.SetFloat("vely", velocity.y);

            LookAt lookAt = GetComponent<LookAt>();
            if (lookAt)
                lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;

            if (!Moving)
                setState(new Idle(this));

            //// Pull character towards agent
            //if (worldDeltaPosition.magnitude > agent.radius)
            //    transform.position = agent.nextPosition - 0.9f * worldDeltaPosition;

            //// Pull agent towards character
            //if (worldDeltaPosition.magnitude > agent.radius)
            //    agent.nextPosition = transform.position + 0.9f * worldDeltaPosition;
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
            target = hit.transform;
            Vector3 targetPosition = hit.point;
            if (currentState is Idle)
            {
                agent.CalculatePath(targetPosition, playerPath);
                if (!(agent.path.status == NavMeshPathStatus.PathComplete))
                {
                    agent.ResetPath();
                    return;
                }
                agent.SetPath(playerPath);
                setState(new Move(this));
            }
        }

        public void setState(State state)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = state;
            currentState.Enter();
        }

        private bool IsInteractable(GameObject gameObject)
        {
            if (gameObject.CompareTag("Interactable"))
            {
                return true;
            }
            return false;
        }

        public bool IsFacingTarget(Transform target)
        {
            Vector3 targetDirection = target.transform.position - transform.position;
            targetDirection.y = 0;
            Vector3 forward = transform.forward;
            forward.y = 0;
            float angle = Vector3.Angle(targetDirection, transform.forward);
            Debug.DrawLine(transform.position, angle * transform.forward);
            return angle < 15f;
        }
        public void RotateTowardsTarget(Transform target)
        {
            float rotationSpeed = 2.5f;

            Vector3 targetDirection = target.transform.position - transform.position;
            targetDirection.y = 0;
            Vector3 forward = transform.forward;
            forward.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    }
}
