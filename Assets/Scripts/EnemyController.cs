using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    // The target the enemy should move towards
    public Transform target;

    // The distance the enemy can see
    public float viewDistance = 10f;

    // The angle of the enemy's field of view
    public float viewAngle = 90f;

    // The speed at which the enemy looks around
    public float lookSpeed = 10f;

    // A reference to the enemy's Nav Mesh Agent component
    private NavMeshAgent agent;

    // The current state of the enemy's behavior
    private IEnemyState currentState, idleState, chaseState, alertState;

    private Animator animator;

    // Value to store the last known position of the player
    private Vector3 PlayerLastPos;

    // Called when the script is first enabled
    private void Start()
    {
        // Get a reference to the enemy's Nav Mesh Agent component
        agent = GetComponent<NavMeshAgent>();
        // create instances of the states and set initial state
        animator = GetComponent<Animator>();
        idleState = new IdleState(this);
        chaseState = new ChaseState(this);
        alertState = new AlertState(this);
        currentState = idleState;
        currentState.OnEnter();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the current state of the enemy's behavior
        currentState.Update();
    }

    // An interface for the enemy's states
    private interface IEnemyState
    {
        void OnEnter();
        void Update();
        void OnExit();
    }

    private void ChangeState(IEnemyState state)
    {
        currentState.OnExit();
        currentState = state;
        currentState.OnEnter();
    }

    // A state for when the enemy is idle
    private class IdleState : IEnemyState
    {
        // A reference to the enemy controller
        EnemyController controller;

        // Constructor for the EnemyIdleState class
        public IdleState(EnemyController controller)
        {
            this.controller = controller;
        }
        // Called when the state is first entered
        public void OnEnter()
        {
            // Start the coroutine that periodically checks if the player is within the enemy's field of view
            // Play the animation clip
            controller.animator.enabled = true;
            controller.animator.Play("idle");
            controller.StartCoroutine(LookForPlayer());
            Debug.Log("Entered Idle");
        }

        // Called once per frame while the state is active
        public void Update()
        {
            // The enemy is idle, so do nothing
            controller.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
            
        }

        // Called when the state is exited
        public void OnExit()
        {
            // Stop
            controller.animator.enabled = false;
            controller.StopCoroutine(LookForPlayer());
            Debug.Log("Left Idle");
        }
        // Coroutine for searching for player
        IEnumerator LookForPlayer()
        {
            while (true)
            {
                // get the forward vector of this game object
                Vector3 forwardVector = controller.transform.forward;

                // draw a debug line in the direction of the forward vector
                Debug.DrawRay(controller.transform.position, forwardVector * 10, Color.red);

                if (controller.EnemySeesPlayer())
                {
                    controller.ChangeState(controller.chaseState);
                    yield break;
                }
                yield return null;
            }
        }

    }
    // A state for when the enemy is Chase
    private class ChaseState : IEnemyState
    {
        // A reference to the enemy controller
        EnemyController controller;

        // Constructor for the EnemyChaseState class
        public ChaseState(EnemyController controller)
        {
            this.controller = controller;
        }
        // Called when the state is first entered
        public void OnEnter()
        {
            // Start the coroutine that periodically checks if the player is within the enemy's field of view
            controller.StartCoroutine(ChasePlayer());
            Debug.Log("Entered Chase");
        }

        // Called once per frame while the state is active
        public void Update()
        {
            // The enemy is Chase, so do nothing
            controller.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            
        }

        // Called when the state is exited
        public void OnExit()
        {
            // Stop
            controller.StopCoroutine(ChasePlayer());
            Debug.Log("Left Chase");
        }
        // Coroutine for searching for player
        IEnumerator ChasePlayer()
        {
            while (true)
            {
                // get the forward vector of this game object
                Vector3 forwardVector = controller.transform.forward;

                // draw a debug line in the direction of the forward vector
                Debug.DrawRay(controller.transform.position, forwardVector * 10, Color.red);

                if (controller.EnemySeesPlayer())
                {
                    controller.agent.SetDestination(controller.target.position);    
                    yield return new WaitForSeconds(5);           
                } else
                {
                    controller.ChangeState(controller.idleState);
                    yield break;
                }
            }
        }

    }
    // A state for when the enemy is Alert
    private class AlertState : IEnemyState
    {
        // A reference to the enemy controller
        EnemyController controller;
        Vector3 position;

        // Constructor for the EnemyAlertState class
        public AlertState(EnemyController controller)
        {
            this.controller = controller;
            this.position = controller.target.position;
        }
        // Called when the state is first entered
        public void OnEnter()
        {
            // Start the coroutine that periodically checks if the player is within the enemy's field of view
            controller.StartCoroutine(AlertPlayer());
            Debug.Log("Entered Alert");
        }

        // Called once per frame while the state is active
        public void Update()
        {
            // The enemy is Alert, so do nothing
            controller.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);

            
        }

        // Called when the state is exited
        public void OnExit()
        {
            // Stop
            controller.StopCoroutine(AlertPlayer());
            Debug.Log("Left Alert");
        }
        // Coroutine for searching for player
        IEnumerator AlertPlayer()
        {
            while (true)
            {
                Vector3 dir = controller.target.position - controller.transform.position;
                dir.y = 0;//This allows the object to only rotate on its y axis
                Quaternion rotation = Quaternion.LookRotation(dir);
                controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, rotation, 5.0f * Time.deltaTime);

                if (controller.IsEnemyLookingAtPosition(controller.transform.position, controller.PlayerLastPos, 5.0f))
                {
                    controller.ChangeState(controller.chaseState);
                    yield break;
                }

                yield return null;
            }
        }



    }
    // A state for when the enemy has Lost the player
    private class LostState : IEnemyState
    {
        // A reference to the enemy controller
        EnemyController controller;

        // Constructor for the EnemyChaseState class
        public LostState(EnemyController controller)
        {
            this.controller = controller;
        }
        // Called when the state is first entered
        public void OnEnter()
        {
            // Start the coroutine that periodically checks if the player is within the enemy's field of view
            controller.StartCoroutine(GoToPlayerLastPos());
            Debug.Log("Entered Chase");
        }

        // Called once per frame while the state is active
        public void Update()
        {
            // The enemy is Chase, so do nothing
            controller.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            
        }

        // Called when the state is exited
        public void OnExit()
        {
            // Stop
            controller.StopCoroutine(GoToPlayerLastPos());
            Debug.Log("Left Chase");
        }
        // Coroutine for searching for player
        IEnumerator GoToPlayerLastPos()
        {
            while (true)
            {
                // get the forward vector of this game object
                Vector3 forwardVector = controller.transform.forward;

                // draw a debug line in the direction of the forward vector
                Debug.DrawRay(controller.transform.position, forwardVector * 10, Color.red);

                if (controller.EnemySeesPlayer())
                {
                    controller.ChangeState(controller.chaseState);
                    yield break;               
                } else
                {
                    if (controller.HasReachedDestination(controller.agent))
                    {
                        controller.ChangeState(controller.idleState);
                        yield break;       
                    }
                    controller.agent.SetDestination(controller.PlayerLastPos);    
                    yield return new WaitForSeconds(5);
                }
            }
        }

    }
    private bool IsEnemyLookingAtPosition (Vector3 EnemyPos, Vector3 targetPos, float maxAngle)
    {
        Vector3 dir = targetPos - EnemyPos;
        float angle = Vector3.Angle(dir, transform.forward);
        if (angle <= maxAngle)
            return true;
        else
            return false;
    }
    GameObject EnemySeesPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 25f);
        
        foreach (Collider collider in hitColliders)
        {
            Vector3 direction = collider.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, direction);

            if (angle < 90f && collider.gameObject.tag == "Player")
            {
                PlayerLastPos = transform.position;
                return collider.gameObject;
            }
        }

        return null;
    }
    private bool HasReachedDestination(NavMeshAgent agent)
{
    if(agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
    {
        return true;
    }
    return false;
}
}