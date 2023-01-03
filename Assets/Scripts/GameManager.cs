using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour 
{
    public NavMeshAgent playerNavMeshAgent;
    
    // Declare a static instance of the GameManager class
    public static GameManager instance;

    // Declare a public field for the UI canvas
    public Transform uiCanvas;
    // Declare a flag to track the pause state
    bool isPaused = false;
    public IEnumerator task;

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
    }

    
    void Update()
    {
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
    }

    private void playerAction(IEnumerator task)
    {
        if (readyForTask(task))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetLocation = hit.point;
                GameObject interactable = hit.transform.gameObject;

                if (interactable.tag == "Interactable")
                {
                    interactable.SendMessage("Interact", interactable);
                }
                // Set the target position for the nav mesh agent
                if (interactable.tag== "Ground")
                {
                    playerNavMeshAgent.SetDestination(targetLocation);
                }
                
            }
        }
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

    public void navToTarget(Transform target)
    {
        playerNavMeshAgent.SetDestination(target.position);
        // Debug.Log("Interact! " + target.position);
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

