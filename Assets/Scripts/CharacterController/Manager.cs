using System;
using UnityEngine;
using UnityEngine.AI;
namespace HeyAlexi.Character
{
    public class Manager : MonoBehaviour
    {
        private Animator anim;
        public NavMeshAgent agent;
        private NavMeshPath playerPath;
        Vector2 smoothDeltaPosition = Vector2.zero;
        Vector2 velocity = Vector2.zero;
        public State currentState;
        public Interactable InteractTarget;
        public Vector3 targetPos;

        void Start()
        {
            anim = GetComponentInChildren<Animator>();
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.updatePosition = false;
            anim.applyRootMotion = true;
            agent.updateRotation = false;
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
            worldDeltaPosition.y = 0;

            ////// Map 'worldDeltaPosition' to local space
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            //////// Low-pass filter the deltaMove
            float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

            //// Update velocity if delta time is safe
            if (Time.deltaTime > 1e-5f)
                velocity = smoothDeltaPosition / Time.deltaTime;

            bool Moving = agent.remainingDistance * 2.0f > agent.radius;

            //// Update animation parameters
            anim.SetBool("move", Moving);
            anim.SetFloat("velx", velocity.x);
            anim.SetFloat("vely", velocity.y);

            LookAt lookAt = GetComponent<LookAt>();
            if (lookAt)
                lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;

            if (!Moving)
            {
                if (InteractTarget)
                {
                    InteractTarget.Trigger();
                    InteractTarget = null;
                }
                setState(new Idle(this));
            }


            //// Pull character towards agent
            if (worldDeltaPosition.magnitude > agent.radius / 2f)
            {
                transform.position = Vector3.Lerp
                (
                    anim.rootPosition,
                    agent.nextPosition,
                    smooth
                );
            }
        }

        public void CharacterStop()
        {
            anim.SetBool("move", false);

        }

        void OnAnimatorMove()
        {
            // Update postion to agent position
            transform.position = agent.nextPosition;

            // Update position based on animation movement using navigation surface height
            Vector3 position = anim.rootPosition;
            position.y = agent.nextPosition.y;
            transform.position = position;
            transform.rotation = anim.rootRotation;

        }

        // Transform target, Vector3 targetPosition
        public void NavToTarget(Vector3 hit)
        {
            playerPath ??= agent.path;
            Vector3 targetPosition = hit;
            if (currentState is not Idle)
            {
                agent.CalculatePath(targetPosition, playerPath);
                if (!(agent.path.status == NavMeshPathStatus.PathComplete))
                {
                    agent.ResetPath();
                    setState(new Idle(this));
                    return;
                }
                agent.SetPath(playerPath);
            }
        }

        public void SetTarget(RaycastHit hit)
        {
            if (IsInteractable(hit.transform.gameObject))
            {              
                InteractTarget = hit.transform.gameObject.GetComponent<Interactable>();
                GameObject target = InteractTarget.FindNearestInteractionZone();
                targetPos = target.transform.position;
                setState(new Move(this));
                return;
            }
            else
            {
                if (currentState is Idle)
                {
                    targetPos = hit.point;
                    setState(new Move(this));
                }
                if (currentState is Moving)
                {
                    setState(new Idle(this));
                }

            }
            return;
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

        public bool IsFacingPos(Vector3 pos)
        {
            Vector3 targetDirection = pos - transform.position;
            targetDirection.y = 0;
            Vector3 forward = transform.forward;
            forward.y = 0;
            float angle = Vector3.Angle(targetDirection, transform.forward);
            Debug.DrawLine(transform.position, angle * transform.forward);
            return angle < 5f;
        }
        public void RotateTowardsPos(Vector3 pos)
        {
            float rotationSpeed = 2.5f;

            Vector3 targetDirection = pos - transform.position;
            targetDirection.y = 0;
            Vector3 forward = transform.forward;
            forward.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    }
}
