using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using HeyAlexi.Character;

namespace HeyAlexi
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Character.Manager _playerPrefab;
        private Character.Manager player;
        public TextMeshProUGUI innerThought;
        // Declare a static instance of the GameManager class
        public static GameManager instance;
        // Declare a public field for the UI canvas
        public Transform uiCanvas;
        // Declare a flag to track the pause state
        bool isPaused = false;

        // Start is called before the first frame update
        void Start()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                // Access the UI canvas from the GameManager
                // Find the Canvas GameObject
                GameObject canvasObject = GameObject.Find("Canvas");

                // Get the Canvas component from the Canvas GameObject
                uiCanvas = canvasObject.transform.Find("PauseMenu");
                player = Instantiate(_playerPrefab);
                DontDestroyOnLoad(gameObject);
            }
        }


        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Player clicked so move or interact with something
                // PlayerAction(instance.task);
                if (player.currentState is Idle)
                {
                    player.NavToTarget(GetMouseClickTarget());
                }
                if (player.currentState is Move)
                {
                    player.setState(new Idle(player));
                    player.NavToTarget(GetMouseClickTarget());
                }

            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Fire the Pause function
                Pause();
            }

            // if (debug)
            // {
            //     // Some debug stuff, mainly draws a line showing the current path.
            //     if (!(player.agent.PlayerPath == null))
            //     {
            //         Vector3[] waypoints = PlayerPath.corners;


            //         if (waypoints.Length > 0)
            //         {
            //             // Set the positions of the LineRenderer to the array of waypoints
            //             lineRenderer.positionCount = waypoints.Length;
            //             lineRenderer.SetPositions(waypoints);
            //         }
            //         else
            //         {
            //             // If the agent doesn't have a path, hide the LineRenderer
            //             lineRenderer.positionCount = 0;
            //         }
            //     }

            // }

            // PlayerPath ??= playerNavMeshAgent.path;
        }

        private RaycastHit GetMouseClickTarget()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit;
            }
            return new RaycastHit();
        }

        public void LoadLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        // Pause function
        void Pause()
        {
            // Toggle the pause state
            isPaused = !isPaused;

            // Pause or unpause the game based on the pause state
            Time.timeScale = isPaused ? 0 : 1;

            // Show or hide the pause menu based on the pause state
            uiCanvas.gameObject.SetActive(isPaused);
        }

        public void SwitchActiveCamera(Camera newActiveCamera)
        {
            // Deactivate all cameras in the scene
            Camera[] allCameras = Camera.allCameras;
            foreach (Camera camera in allCameras)
            {
                camera.gameObject.SetActive(false);
            }

            // Activate the new active camera
            newActiveCamera.gameObject.SetActive(true);
        }

        public GameObject GetNearestToPlayer(List<GameObject> gameObjects)
        {
            GameObject player = GameObject.Find("Player");

            GameObject closestGameObject = null;
            float closestDistance = Mathf.Infinity;

            foreach (GameObject gameObject in gameObjects)
            {
                float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
                if (distance < closestDistance)
                {
                    closestGameObject = gameObject;
                    closestDistance = distance;
                }
            }

            return closestGameObject;
        }

        public GameObject GetFurthestToPlayer(List<GameObject> gameObjects)
        {
            GameObject player = GameObject.Find("Player");

            GameObject furthestGameObject = null;
            float furthestDistance = Mathf.Infinity;

            foreach (GameObject gameObject in gameObjects)
            {
                float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
                if (distance > furthestDistance)
                {
                    furthestGameObject = gameObject;
                    furthestDistance = distance;
                }
            }

            return furthestGameObject;
        }

        public bool IsFacingTarget(Transform target)
        {
            Vector3 targetDirection = target.transform.position - player.transform.position;
            targetDirection.y = 0;
            Vector3 forward = transform.forward;
            forward.y = 0;
            float angle = Vector3.Angle(targetDirection, player.transform.forward);
            Debug.DrawLine(player.transform.position, angle * player.transform.forward);
            return angle < 15f;
        }

        public void RotateTowardsTarget(Transform target)
        {
            float rotationSpeed = 2.5f;
            
            Vector3 targetDirection = target.transform.position - player.transform.position;
            targetDirection.y = 0;
            Vector3 forward = transform.forward;
            forward.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }        

        // IEnumerator Task(RaycastHit hit)
        // {
        //     while(!(IsFacingTarget(hit.transform)))
        //     {
        //         RotateTowardsTarget(hit.transform);
        //         yield return null;
        //     }

        //     if (IsInteractable(hit.transform.gameObject))
        //     {
        //         hit.transform.gameObject.GetComponent<Interactable>().Interact(hit);
        //         yield break;
        //     }

        //     NavToTarget(hit.transform, hit.point);

        //     while(!(GameManager.instance.task == null))
        //     {
        //         if (!playerNavMeshAgent.pathPending)
        //         {
                    
        //             if (playerNavMeshAgent.remainingDistance <= playerNavMeshAgent.stoppingDistance)
        //             {
        //                 if (!playerNavMeshAgent.hasPath || playerNavMeshAgent.velocity.sqrMagnitude == 0f)
        //                 {
        //                     instance.task = null;
        //                     yield break;
        //                 }
        //             }
        //         }
        //         yield return null;
        //     }
        // }

    }
}