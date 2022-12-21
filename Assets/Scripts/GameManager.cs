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
    public Canvas uiCanvas;
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
            uiCanvas = canvasObject.GetComponent<Canvas>();
            DontDestroyOnLoad(gameObject);
        }
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetLocation = hit.point;

                // Set the target position for the nav mesh agent
                playerNavMeshAgent.SetDestination(targetLocation);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Fire the Pause function
            Pause();
        }
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

}

