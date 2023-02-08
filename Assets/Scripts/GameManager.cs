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
        public Transform PauseMenu;
        public Transform ItemMenu;
        // Declare a flag to track the pause state
        bool isPaused = false;
        bool isItemMenuOpen = false;

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
                PauseMenu = canvasObject.transform.Find("PauseMenu");
                ItemMenu = canvasObject.transform.Find("ItemMenu");
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
                player.SetTarget(GetMouseClickTarget());
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Fire the Pause function
                Pause();
            }
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
            PauseMenu.gameObject.SetActive(isPaused);
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

        public void ExitItemMenu()
        {
            // Show or hide the item menu
            ItemMenu.gameObject.SetActive(false);
        }

        public void OpenItemMenu()
        {
            // Show or hide the item menu
            ItemMenu.gameObject.SetActive(true);
        }
    }
}