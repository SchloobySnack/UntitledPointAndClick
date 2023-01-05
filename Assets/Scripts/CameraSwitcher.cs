using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeyAlexi
{
    public class CameraSwitcher : MonoBehaviour
    {
        public Camera newActiveCamera; // Drag the new active camera from the hierarchy onto this public variable in the Inspector

        private void OnTriggerEnter(Collider collider)
        {
            // Check if the colliding object has the "Player" tag
            if (collider.gameObject.tag == "Player")
            {
                // Get a reference to the GameManager in the scene
                GameManager gameManager = FindObjectOfType<GameManager>();

                // Call the SwitchActiveCamera function in the GameManager, passing in the new active camera as an argument
                gameManager.SwitchActiveCamera(newActiveCamera);
            }
        }
    }
}