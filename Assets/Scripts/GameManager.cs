using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GameManager : MonoBehaviour 
{
    public NavMeshAgent playerNavMeshAgent;
    
    // Declare a static instance of the GameManager class
    public static GameManager instance;

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
    }

}

