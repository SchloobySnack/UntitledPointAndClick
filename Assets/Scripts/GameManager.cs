using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour 
{
    public NavMeshAgent playerNavMeshAgent;
    public bool debug;
    public TextMeshProUGUI innerThought;
    // Declare a static instance of the GameManager class
    public static GameManager instance;

    // Declare a public field for the UI canvas
    public Transform uiCanvas;
    // Declare a flag to track the pause state
    bool isPaused = false;
    public IEnumerator task;
    private NavMeshPath PlayerPath;

    LineRenderer lineRenderer;

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
            uiCanvas =  canvasObject.transform.Find("PauseMenu");
            DontDestroyOnLoad(gameObject);
        }

        lineRenderer = GetComponent<LineRenderer>();
    }

    
    void Update()
    {
        Debug.Log("Update was called");
        if (Input.GetMouseButtonDown(0))
        {
            // Player clicked so move or interact with something
            playerAction(GameManager.instance.task);                
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Fire the Pause function
            Pause();
        }

        if (debug)
        {
            // Some debug stuff, mainly draws a line showing the current path.
            if (!(PlayerPath == null))
            {
                Vector3[] waypoints = PlayerPath.corners;
            

                if (waypoints.Length > 0) {
                    // Set the positions of the LineRenderer to the array of waypoints
                    lineRenderer.positionCount = waypoints.Length;
                    lineRenderer.SetPositions(waypoints);
                } else {
                    // If the agent doesn't have a path, hide the LineRenderer
                    lineRenderer.positionCount = 0;
                }
            }
            
        }

        if (PlayerPath == null)
        {
            PlayerPath = playerNavMeshAgent.path;
        }



    }

    private void playerAction(IEnumerator task)
    {
        if (readyForTask(task))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (Physics.Raycast(ray, out hit))
            {
                startNewTask(hit);
                return;
            }
            PlayerPath = null;
        }
    }

    private void startNewTask(RaycastHit hit)
    {
        if (isInteractable(hit.transform.gameObject))
        {
            hit.transform.gameObject.GetComponent<Interactable>().Interact(hit);
            return;
        }
        navToTarget(hit.transform, hit.point);
    }

    private bool isInteractable(GameObject gameObject)
    {
        if (gameObject.tag == "Interactable")
        {
            return true;
        }
        return false;

    }
    private bool readyForTask(IEnumerator task)
    {
        if (!(task == null))
        {
            StopCoroutine(task);
        }        
        playerNavMeshAgent.ResetPath();
        GameManager.instance.task = null;
        return true;
    }
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void navToTarget(Transform target, Vector3 targetPosition)
    {

        playerNavMeshAgent.CalculatePath(targetPosition, PlayerPath);
        if (!(PlayerPath.status == NavMeshPathStatus.PathComplete))
        {
            PlayerPath = null;
            return;
        }
        Vector3[] waypoints = PlayerPath.corners;
        
        // looking at the first waypoint position in one frame. May want to tween this later.
        playerNavMeshAgent.transform.LookAt(targetPosition);
        playerNavMeshAgent.SetPath(PlayerPath);
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

}

